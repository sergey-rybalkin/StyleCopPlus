#Requires -Version 7.0
<#
    .SYNOPSIS
        Creates new git tag with incremented application version as its name as well as updates version number
         in source files.
    .DESCRIPTION
        Looks for the most recent release tag and creates a new one with incremented major or minor number.
        Also looks for source and configuration files that contain version number and updates them as well.
        Assumes release tags are created in major.minor format, e.g. 12.2.
    .PARAMETER IncrementMajor
        Indicates whether to increment major version component.
    .PARAMETER IncrementMinor
        Indicates whether to increment minor version component.
    .EXAMPLE
        Set-ReleaseTag -IncrementMajor
#>

[CmdletBinding(PositionalBinding = $true, SupportsShouldProcess)]
param(
    [switch] $IncrementMajor = $false,
    [switch] $IncrementMinor = $false,
    [string] $TagAnnotation
)

Set-StrictMode -Version 'Latest'
$ErrorActionPreference = 'Stop'

# Check prerequisites
$allowedHosts = @('ConsoleHost', 'Visual Studio Code Host')

if (-not ($Host.Name -in $allowedHosts)) {
    Write-Error "Host '$($Host.Name)' is not supported by this script"
    exit 1
}

# Do not use conditional statement here as this action is not destructive and is required by the rest of the
# script.
$pscmdlet.ShouldProcess("repository $(Get-Location)", 'git describe') | Out-Null
$gitDescribeOutput = & git describe --abbrev=0

if (-not ($gitDescribeOutput -match '^(\d+)\.(\d+)$')) {
    Write-Error "Unexpected git describe command output: ${gitDescribeOutput}"
}

$majorVersion = $Matches[1] -as [Int32]
$minorVersion = $Matches[2] -as [Int32]

if ($IncrementMajor) {
    $majorVersion++
}

if ($IncrementMinor) {
    $minorVersion++
}

if (!$TagAnnotation) {
    $TagAnnotation = "Version ${majorVersion}.${minorVersion}"
}

# Create annotated git tag with an updated version
if ($pscmdlet.ShouldProcess("repository $(Get-Location)", "git tag ${majorVersion}.${minorVersion}")) {
    & git tag -a "${majorVersion}.${minorVersion}" -m "${TagAnnotation}"
}

# Update version number in source and configuration files
if ($pscmdlet.ShouldProcess('StyleCopPlus.csproj', "Update nuget package version")) {
    (Get-Content ..\src\StyleCopPlus\StyleCopPlus.csproj) -replace 'Version>\d+\.\d+', "Version>${majorVersion}.${minorVersion}" |
    Set-Content ..\src\StyleCopPlus\StyleCopPlus.csproj
}

if ($pscmdlet.ShouldProcess('VSIX package', "Update VSIX package version")) {
    (Get-Content ..\src\StyleCopPlus.Vsix\source.extension.vsixmanifest) -replace 'Version="\d+\.\d+', "Version=""${majorVersion}.${minorVersion}" |
    Set-Content ..\src\StyleCopPlus.Vsix\source.extension.vsixmanifest

    (Get-Content ..\src\StyleCopPlus.Vsix\source.extension.cs) -replace '5d32dfd0e204" Version = "\d+\.\d+', "5d32dfd0e204"" Version = ""${majorVersion}.${minorVersion}" |
    Set-Content ..\src\StyleCopPlus.Vsix\source.extension.cs
}