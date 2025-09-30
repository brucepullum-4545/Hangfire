@echo off
echo Building HangFireProj...

REM Restore NuGet packages
echo Restoring NuGet packages...
nuget restore HangFireProj.sln

REM Build the project
echo Building project...
msbuild HangFireProj.csproj /p:Configuration=Debug /p:Platform="Any CPU"

if %ERRORLEVEL% EQU 0 (
    echo Build successful!
) else (
    echo Build failed with error code %ERRORLEVEL%
)

pause
