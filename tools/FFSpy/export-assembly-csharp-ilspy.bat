@echo off
setlocal

rem ILSpy export — browse/reference ONLY. Output is NOT safe for dnSpy whole-class paste.
rem Use export-assembly-csharp.bat (dnSpy format) for files you will paste back into dnSpy.

set "PATH=%PATH%;%ProgramFiles%\dotnet;%USERPROFILE%\.dotnet\tools"
set "DLL=D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\Assembly - CSharp.dll"
set "OUT=D:\work\roberto\mods\decompiled-ilspy\Assembly-CSharp"

if not exist "%DLL%" (
    echo DLL not found:
    echo   %DLL%
    echo Run bundle-extract first, or edit the DLL path in this script.
    exit /b 1
)

where ilspycmd >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo ilspycmd not found. Install with:
    echo   dotnet tool install ilspycmd --version 8.2.0.7535 -g
    exit /b 1
)

if not exist "%OUT%" mkdir "%OUT%"

echo Exporting (ILSpy reference format — NOT for dnSpy paste):
echo   %DLL%
echo To:
echo   %OUT%
echo.

ilspycmd "%DLL%" -p -o "%OUT%" --nested-directories -lv CSharp7_3
if %ERRORLEVEL% neq 0 (
    echo Export failed.
    exit /b %ERRORLEVEL%
)

echo.
echo Export complete. Use for reading/search only.
echo For dnSpy-compatible export run:
echo   tools\FFSpy\export-assembly-csharp.bat
exit /b 0
