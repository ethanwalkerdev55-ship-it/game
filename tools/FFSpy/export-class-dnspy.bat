@echo off
setlocal EnableDelayedExpansion

rem Export a single class in dnSpy round-trip format.
rem Usage: export-class-dnspy.bat cnMissionManager

set "DNSPY_CONSOLE=%~dp0..\dnSpyEx\dnSpy.Console.exe"
if not "%CLIENT_DLL%"=="" (
    set "DLL=%CLIENT_DLL%"
) else (
    set "DLL=D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\Assembly - CSharp.dll"
)
set "CLASS=%~1"
set "OUT=D:\work\roberto\mods\decompiled\Assembly-CSharp"
set "TMP=%OUT%\_dnspy_class_tmp"

if "%CLASS%"=="" (
    echo Usage: export-class-dnspy.bat ClassName
    echo Example: export-class-dnspy.bat cnMissionManager
    exit /b 1
)

if not exist "%DNSPY_CONSOLE%" (
    echo dnSpy.Console.exe not found. Run install-dnspy-export.bat first.
    exit /b 1
)

if not exist "%DLL%" (
    echo DLL not found: %DLL%
    exit /b 1
)

if not exist "%OUT%" mkdir "%OUT%"
if exist "%TMP%" rmdir /s /q "%TMP%"
mkdir "%TMP%"

echo Exporting class %CLASS% (dnSpy format) ...
"%DNSPY_CONSOLE%" -o "%TMP%" --no-yield --tokens --spaces 4 "%DLL%" >nul
if %ERRORLEVEL% neq 0 (
    echo Export failed.
    rmdir /s /q "%TMP%" 2>nul
    exit /b %ERRORLEVEL%
)

set "FOUND=0"
for /r "%TMP%" %%F in (%CLASS%.cs) do (
    copy /Y "%%F" "%OUT%\%CLASS%.cs" >nul
    echo Wrote: %OUT%\%CLASS%.cs
    set "FOUND=1"
    goto :done
)

:done
rmdir /s /q "%TMP%" 2>nul
if "%FOUND%"=="0" (
    echo Class %CLASS%.cs not found after export.
    exit /b 1
)
exit /b 0
