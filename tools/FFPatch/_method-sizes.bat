@echo off
set "CLIENT=D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll"
set "FF=D:\work\roberto\tools\FFPatch\bin\Release\net48\FFPatch.exe"
"%FF%" method-sizes "%CLIENT%" ForceCompleteCurrentTask RequestTaskComplete
