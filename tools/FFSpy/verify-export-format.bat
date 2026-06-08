@echo off
setlocal

rem Quick check: is cnMissionManager.cs dnSpy-safe or ILSpy/broken-recompile style?

set "FILE=D:\work\roberto\mods\decompiled\Assembly-CSharp\cnMissionManager.cs"

if not exist "%FILE%" (
    echo cnMissionManager.cs not found. Run export-assembly-csharp.bat first.
    exit /b 1
)

findstr /C:"Object.op_Implicit" "%FILE%" >nul && set "ILSPY=1" || set "ILSPY=0"
findstr /C:"yield return new WaitForSeconds" "%FILE%" >nul && set "YIELD=1" || set "YIELD=0"
findstr /C:"<CallGuideMessage>d__" "%FILE%" >nul && set "NESTED=1" || set "NESTED=0"
findstr /C:"// Token: 0x" "%FILE%" >nul && set "TOKEN=1" || set "TOKEN=0"

echo Checking: %FILE%
echo.

if "%NESTED%"=="1" if "%TOKEN%"=="1" (
    echo OK — dnSpy round-trip format ^(nested coroutines + Token comments^).
    exit /b 0
)

if "%ILSPY%"=="1" (
    echo BAD — ILSpy style detected ^(Object.op_Implicit^).
    echo Re-export with: tools\FFSpy\export-assembly-csharp.bat
    exit /b 2
)

if "%YIELD%"=="1" (
    echo BAD — yield-return coroutines detected.
    echo Either the DLL was whole-class recompiled in dnSpy, or export used ILSpy.
    echo Fix: restore vanilla DLL, bundle-extract from .bak, then export-assembly-csharp.bat
    exit /b 3
)

echo WARN — format unclear. Prefer nested d__ coroutines and // Token comments.
exit /b 4
