<#
.SYNOPSIS
StyleCopPlus analyzers build script.

.DESCRIPTION
Builds specified configuration of StyleCopPlus analyzers and optionally installs nuget package to the local feed.

#>
[CmdletBinding()]
PARAM (
    [string]$Configuration = "Release",

    [switch]$Install = $true
)

# Ensure that we have MSBuild logs folder
if (-Not (Test-Path logs)) {
    mkdir logs
}

$MSBuildCmd = "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
& $MSBuildCmd "StyleCopPlus.Analyzers.sln" /m /nr:false /t:Build "/p:Configuration=$Configuration" /v:M /fl "/flp:LogFile=logs\msbuild.log;Verbosity=Normal"

if ($LASTEXITCODE -eq 0 -and $Install) {
    $Package = Get-ChildItem -Recurse -Path "src\StyleCopPlus.Analyzers\bin\Release\" -Filter "*.nupkg" | select -last 1
    & "NuGet.exe" add $Package.FullName -source "D:\dev\_nuget"
}