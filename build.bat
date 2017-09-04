@echo off
powershell.exe -NoProfile -ExecutionPolicy unrestricted -command ".\build.ps1 %1 %2 %3 %4 %5;exit $LASTEXITCODE";

if %ERRORLEVEL% == 0 goto OK
    echo ##teamcity[buildStatus status="FAILURE" text="{build.status.text} in compilation"]

exit /B %ERRORLEVEL%

:OK