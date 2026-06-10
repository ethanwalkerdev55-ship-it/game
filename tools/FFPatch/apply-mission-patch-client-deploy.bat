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

echo === Deploy mission patch (SAFE - bundle-inject-preserve) ===
echo Slot-padded DLL + UnityWeb preserve-inject (lastU gate, no UnityRaw).
echo Fix-forward only — do not restore-client.bat on bootstrap failure.

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

for %%F in ("%CLIENT_DLL%") do set "CLIENT_DLL_SIZE=%%~zF"
for %%F in ("%STAGED%") do set "STAGED_DLL_SIZE=%%~zF"
if not %STAGED_DLL_SIZE%==%CLIENT_DLL_SIZE% (
    echo FAIL: staged DLL %STAGED_DLL_SIZE% must equal client slot %CLIENT_DLL_SIZE% bytes.
    echo Re-run apply-mission-patch-staging.bat ^(dll-slot-fit^).
    exit /b 1
)

call "%~dp0bundle-inject-preserve.bat" "%BUNDLE%" "%CLIENT_BUNDLE%"
if %ERRORLEVEL% neq 0 (
    echo FAIL: bundle-inject-preserve — fix slot fit or layout, do not restore.
    exit /b %ERRORLEVEL%
)

for %%F in ("%CLIENT_BUNDLE%") do set "CLIENT_SIZE=%%~zF"
for %%F in ("%BUNDLE%") do set "PATCHED_SIZE=%%~zF"
if not %PATCHED_SIZE%==%CLIENT_SIZE% (
    echo NOTE: bundle size after preserve-inject: %PATCHED_SIZE% vs client %CLIENT_SIZE%
)

call "%~dp0verify-bundle-patch.bat"
if %ERRORLEVEL% neq 0 exit /b 1

call "%~dp0verify-bundle-layout.bat" "%BUNDLE%"
if %ERRORLEVEL% neq 0 exit /b 1

call "%~dp0sync-patch-cache.bat" --pin
if %ERRORLEVEL% neq 0 exit /b 1

for %%F in ("%BUNDLE%") do set "DEPLOY_SIZE=%%~zF"
for /f "delims=" %%H in ('powershell -NoProfile -Command "(Get-FileHash '%BUNDLE%' -Algorithm SHA256).Hash.ToLower()"') do set "DEPLOY_HASH=%%H"

echo.
echo Deploy finished. Bundle: size=%DEPLOY_SIZE% hash=%DEPLOY_HASH%
echo.
echo BEFORE CONNECT (mandatory — launcher reverts manifest to CDN on page navigation):
echo   1. Fully quit launcher + game if running
echo   2. %~dp0launch-patched-client.bat
echo   3. In launcher: stay on server list, click Connect immediately
echo   4. Log must show: Resources base url + ForceCompleteV2: patch build 2026-06-09-doc504h
echo.
echo Rollback: restore-client.bat
exit /b 0
