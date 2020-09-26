<#
    .SYNOPSIS
        StyleCopPlus analyzers build script.
    .DESCRIPTION
        Builds specified configuration of StyleCopPlus analyzers and optionally installs nuget package to the local feed.
    .PARAMETER Configuration
        Solution configuration (Release, Debug etc.) to build.
    .PARAMETER Install
        Indicates whether to install package to the local NuGet feed.
    .PARAMETER RunTests
        Indicates whether to run unit tests after build.
    .PARAMETER MSBuildLocation
        Full path to MSBuild.exe that should be used to build the project. If omitted then latest installed version will be used.
    .EXAMPLE 
        -Configuraiton Release -Install
#>
[CmdletBinding(PositionalBinding = $true)]
PARAM (
    [Parameter(Position=1)]
    [string] $Configuration = "Release",
    [switch] $Install = $false,
    [switch] $RunTests = $false,
    [string] $MSBuildLocation
)

Set-StrictMode -Version 'Latest'

# Ensure that we have MSBuild logs folder
if (-Not (Test-Path logs)) {
    mkdir logs
}

# Ensure that we have a valid MSBuild tool location
if ([string]::IsNullOrEmpty($MSBuildLocation) -Or !(Test-Path $MSBuildLocation)) {
    # Command line utility that is installed along with Visual Studio and helps locating its components
    $VSWhereLocation = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"

    if (-Not (Test-Path $VSWhereLocation)) {
        throw 'Cannot find MSBuild installation path, please use $MSBuildLocation parameter.'
    }

    $VSLocation = & $VSWhereLocation -latest -requires Microsoft.Component.MSBuild -property installationPath
    $MSBuild = Join-Path -Path $VSLocation -ChildPath "MSBuild\Current\Bin\MSBuild.exe"

    if (!(Test-Path $MSBuild)) {
        throw "Cannot find MSBuild at $MSBuild, please use `$MSBuildLocation parameter."
    } else {
        Write-Verbose "MSBuild.exe found at $MSBuild"
        $MSBuildLocation = $MSBuild
    }
}

& $MSBuildLocation "StyleCopPlus.Analyzers.sln" /m /nr:false /t:Build "/p:Configuration=$Configuration" /v:M /fl "/flp:LogFile=logs\msbuild.log;Verbosity=Normal"

if ($LASTEXITCODE -eq 0 -and $Install) {
    $Package = Get-ChildItem -Recurse -Path "src\StyleCopPlus.Analyzers\bin\Release\" -Filter "*.nupkg" | select -last 1
    & "NuGet.exe" add $Package.FullName -source "D:\dev\_nuget"
}