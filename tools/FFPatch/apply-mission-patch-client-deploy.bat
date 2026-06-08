@echo off
setlocal EnableDelayedExpansion

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "BUNDLE=%ROOT%\%UUID%\main.unity3d"
set "MAIN=%ROOT%\%UUID%\main"
set "CLIENT_BUNDLE=%ROOT%\_inspect_bundle\client\main.unity3d"
set "CLIENT_DLL=%ROOT%\_inspect_bundle\client\Assembly - CSharp.dll"
set "CLIENT_FIRSTPASS=%ROOT%\_inspect_bundle\client\Assembly - CSharp - first pass.dll"
set "TARGET=%MAIN%\Assembly - CSharp.dll"
set "TARGET_FIRSTPASS=%MAIN%\Assembly - CSharp - first pass.dll"
set "FFPATCH=%ROOT%\tools\FFPatch\bin\Release\net48\FFPatch.exe"
set "DISUNITY=%ROOT%\tools\DisunityFF\disunity.bat"
set "STAGED=%ROOT%\tools\FFPatch\staging\Assembly-CSharp.patched.dll"

echo === Deploy mission patch (SAFE) ===
echo Rollback: restore-client.bat

if not exist "%CLIENT_BUNDLE%" (
    echo Client backup missing. Run sync-client-backup.bat first.
    exit /b 1
)

if not exist "%STAGED%" (
    echo Staging DLL missing. Run apply-mission-patch-staging.bat first.
    exit /b 1
)

dotnet build "%ROOT%\tools\FFPatch\FFPatch.csproj" -c Release -v q
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

copy /Y "%CLIENT_BUNDLE%" "%BUNDLE%" >nul
if exist "%MAIN%" rmdir /S /Q "%MAIN%"

call "%DISUNITY%" bundle-extract "%BUNDLE%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

if exist "%CLIENT_FIRSTPASS%" copy /Y "%CLIENT_FIRSTPASS%" "%TARGET_FIRSTPASS%" >nul
copy /Y "%CLIENT_DLL%" "%TARGET%" >nul
copy /Y "%STAGED%" "%TARGET%" >nul

call "%DISUNITY%" bundle-inject "%BUNDLE%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

for %%F in ("%CLIENT_BUNDLE%") do set "CLIENT_SIZE=%%~zF"
for %%F in ("%BUNDLE%") do set "PATCHED_SIZE=%%~zF"
if %PATCHED_SIZE% LSS %CLIENT_SIZE% (
    echo FAIL: bundle-inject shrank main.unity3d (%PATCHED_SIZE% ^< %CLIENT_SIZE% bytes^).
    echo This breaks bootstrap at assetInfo.php. Restoring client backup...
    copy /Y "%CLIENT_BUNDLE%" "%BUNDLE%" >nul
    exit /b 1
)

call "%~dp0fix-launcher-manifest.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

call "%~dp0verify-deploy-manifest.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo.
echo Deploy finished. BEFORE TEST run:
echo   %~dp0launch-patched-client.bat
exit /b 0
