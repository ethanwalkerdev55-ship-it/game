@echo off
setlocal
cd /d "%~dp0"
echo FusionFall UDP log listener (127.0.0.1:5140)
echo Log file: %~dp0fusionfall_log.txt
echo.
echo Start this BEFORE launching the game. Requires client bundle (UdpLogger in first pass DLL).
echo.
python "%~dp0udp_listener.py"
if %ERRORLEVEL% neq 0 (
    echo.
    echo Failed. Try: py -3 "%~dp0udp_listener.py"
    pause
)
