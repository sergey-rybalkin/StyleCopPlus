@echo off
pushd %~dp0
setlocal

if not exist logs mkdir logs

REM Find the most recent 32bit MSBuild.exe on the system.
set MSBuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %MSBuild% @set MSBuild="%ProgramFiles%\MSBuild\14.0\Bin\MSBuild.exe"

set target=%1
if "%target%" == "" @set target=Build

%MSBuild% StyleCopPlus.Analyzers.sln /m /nr:false /t:%target% /v:M /fl /flp:LogFile=%~dp0logs\msbuild.log;Verbosity=Normal
if %ERRORLEVEL% neq 0 goto BuildFail
goto BuildSuccess

:BuildFail
echo.
echo *** BUILD FAILED ***
goto End

:BuildSuccess
echo.
echo **** BUILD SUCCESSFUL ***
goto End

:End
popd
endlocal