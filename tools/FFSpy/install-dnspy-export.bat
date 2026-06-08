@echo off
setlocal

rem Downloads dnSpyEx portable if missing (for dnSpy-format DLL export).

set "TOOLS=%~dp0.."
set "DNSPY_DIR=%TOOLS%\dnSpyEx"
set "ZIP=%TOOLS%\downloads\dnSpyEx-win64.zip"
set "CONSOLE=%DNSPY_DIR%\dnSpy.Console.exe"

if exist "%CONSOLE%" (
    echo dnSpy export tools already installed:
    echo   %CONSOLE%
    exit /b 0
)

echo Downloading dnSpyEx v6.5.1 ...
powershell -NoProfile -Command ^
  "$r = Invoke-RestMethod 'https://api.github.com/repos/dnSpyEx/dnSpy/releases/latest';" ^
  "$a = $r.assets | Where-Object { $_.name -match 'dnSpy-net-win64.*\.zip' } | Select-Object -First 1;" ^
  "Invoke-WebRequest -Uri $a.browser_download_url -OutFile '%ZIP%' -UseBasicParsing;" ^
  "Expand-Archive -Path '%ZIP%' -DestinationPath '%DNSPY_DIR%' -Force"

if not exist "%CONSOLE%" (
    echo Install failed — dnSpy.Console.exe not found.
    exit /b 1
)

echo.
echo Installed:
echo   %CONSOLE%
echo.
echo Run export:
echo   tools\FFSpy\export-assembly-csharp.bat
exit /b 0
