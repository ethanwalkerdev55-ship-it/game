@echo off
setlocal EnableDelayedExpansion

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "BUNDLE=%ROOT%\%UUID%\main.unity3d"
set "MAIN=%ROOT%\%UUID%\main"
set "DLL=%MAIN%\Assembly - CSharp.dll"
set "CLIENT_DLL=%ROOT%\_inspect_bundle\client\Assembly - CSharp.dll"
set "FFPATCH=%ROOT%\tools\FFPatch\bin\Release\net48\FFPatch.exe"
set "DISUNITY=%ROOT%\tools\DisunityFF\disunity.bat"

if not exist "%BUNDLE%" (
    echo FAIL: bundle missing: %BUNDLE%
    exit /b 1
)

if not exist "%DLL%" (
    echo Extracting bundle for DLL verify...
    if exist "%MAIN%" rmdir /S /Q "%MAIN%"
    call "%DISUNITY%" bundle-extract "%BUNDLE%"
    if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%
)

if not exist "%DLL%" (
    echo FAIL: no Assembly - CSharp.dll in %MAIN%
    exit /b 1
)

dotnet build "%ROOT%\tools\FFPatch\FFPatch.csproj" -c Release -v q
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

"%FFPATCH%" verify-il "%DLL%" --lite
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

"%FFPATCH%" verify-bootstrap "%DLL%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

"%FFPATCH%" verify-safe-plus "%DLL%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

if exist "%CLIENT_DLL%" (
    "%FFPATCH%" verify-load-safe "%DLL%" "%CLIENT_DLL%"
    if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%
)

echo verify-bundle-patch: OK (patched DLL inside bundle passes all gates)
exit /b 0
