@echo off
echo Installing Visual C++ Redistributable...
curl -L -o vc_redist.x64.exe https://aka.ms/vs/17/release/vc_redist.x64.exe
start /wait vc_redist.x64.exe /quiet
del vc_redist.x64.exe
echo VC++ Redistributable installed successfully.
