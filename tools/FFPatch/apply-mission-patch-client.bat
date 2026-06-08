@echo off
setlocal

echo Mission patch workflow:
echo   1. ingest-client-bundle.bat        ^(new client main 2.unity3d^)
echo   2. apply-mission-patch-staging.bat
echo   3. run-verify-staging.bat
echo   4. apply-mission-patch-client-deploy.bat
echo   5. launch-patched-client.bat       ^(required every launch^)
echo.
echo Hotkey: F11 or Right Ctrl
echo Rollback: restore-client.bat
echo Logs: _inspect_udp_listener\fusionfall_log.txt
exit /b 0
