@echo off
setlocal

echo.
echo === Mission patch launcher prep ===
echo.
echo Syncs patched bundle + bootstrap to offline cache AND ffcache, then pins manifest.
echo NOTE: game log 'Requesting to assetInfo.php' is request START only — failures are silent.
echo       udp_listener now emits BOOTSTRAP_STALL diagnostics; run bootstrap-snapshot before Connect.
echo.

call "%~dp0sync-patch-cache.bat" --pin
if %ERRORLEVEL% neq 0 (
    echo.
    echo Sync failed. Run: apply-mission-patch-client-deploy.bat
    exit /b 1
)

call "%~dp0bootstrap-snapshot.bat"

echo.
echo Ready. In the launcher:
echo   1. Fully quit and reopen launcher if it was already open
echo   2. Tester1 selected -^> Connect
echo.
echo Rollback: restore-client.bat
exit /b 0

