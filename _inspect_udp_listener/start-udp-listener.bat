@echo off
setlocal
cd /d "%~dp0"
echo FusionFall UDP log listener (127.0.0.1:5140)
echo Log file: %~dp0fusionfall_log.txt
echo.
echo Start this BEFORE launching the game. Requires client bundle (UdpLogger in first pass DLL).
echo.
echo Bootstrap watchdog: if 'Requesting to assetInfo.php' never reaches 'Resources base url',
echo the listener writes BOOTSTRAP_STALL with ffrunner.log tail + bootstrap_context.json.
echo Run tools\FFPatch\bootstrap-snapshot.bat before Connect for best diagnostics.
echo.
python "%~dp0udp_listener.py"
if %ERRORLEVEL% neq 0 (
    echo.
    echo Failed. Try: py -3 "%~dp0udp_listener.py"
    pause
)
