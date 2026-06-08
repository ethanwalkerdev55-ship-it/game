@echo off
setlocal EnableDelayedExpansion

rem dnSpy-format export (safe to paste back into dnSpy Edit Class)
rem Uses --no-yield so coroutines stay as compiler-generated nested classes.

set "BASEDIR=%~dp0"
set "DNSPY_CONSOLE=%~dp0..\dnSpyEx\dnSpy.Console.exe"
set "DLL=D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\Assembly - CSharp.dll"
set "OUT=D:\work\roberto\mods\decompiled\Assembly-CSharp"
set "TMP=%OUT%\_dnspy_export_tmp"

if not exist "%DNSPY_CONSOLE%" (
    echo dnSpy.Console.exe not found:
    echo   %DNSPY_CONSOLE%
    echo Run tools\FFSpy\install-dnspy-export.bat first.
    exit /b 1
)

if not exist "%DLL%" (
    echo DLL not found:
    echo   %DLL%
    echo Run bundle-extract first, or edit the DLL path in this script.
    exit /b 1
)

if not exist "%OUT%" mkdir "%OUT%"
if exist "%TMP%" rmdir /s /q "%TMP%"
mkdir "%TMP%"

echo Exporting (dnSpy format):
echo   %DLL%
echo To:
echo   %OUT%
echo.

"%DNSPY_CONSOLE%" -o "%TMP%" --no-yield --tokens --spaces 4 "%DLL%"
if %ERRORLEVEL% neq 0 (
    echo Export failed.
    exit /b %ERRORLEVEL%
)

rem Flatten hash-named subfolder to project root (Cursor-friendly layout)
set "FLAT=0"
for /d %%D in ("%TMP%\*") do (
    xcopy /Y /Q "%%D\*.cs" "%OUT%\" >nul
    set "FLAT=1"
)
if "%FLAT%"=="0" (
    xcopy /Y /Q "%TMP%\*.cs" "%OUT%\" >nul
)

rmdir /s /q "%TMP%"

echo.
echo Export complete (dnSpy-compatible format).
echo   - Uses this. / base. and // Token comments
echo   - Coroutines are nested classes (--no-yield), not yield return
echo.
echo Open in Cursor:
echo   %OUT%
exit /b 0
