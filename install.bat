@echo off
echo ### 正在请求管理员权限 ###
cd /d "%~dp0"
cacls.exe "%SystemDrive%\System Volume Information" >nul 2>nul
if %errorlevel%==0 goto Admin
if exist "%temp%\getadmin.vbs" del /f /q "%temp%\getadmin.vbs"
echo Set RequestUAC = CreateObject^("Shell.Application"^)>"%temp%\getadmin.vbs"
echo RequestUAC.ShellExecute "%~s0","","","runas",1 >>"%temp%\getadmin.vbs"
echo WScript.Quit >>"%temp%\getadmin.vbs"
"%temp%\getadmin.vbs" /f
if exist "%temp%\getadmin.vbs" del /f /q "%temp%\getadmin.vbs"
exit

:Admin
echo ### 已获取管理员权限，为当前用户设置 Powershell Unrestricted 执行策略 ###
powershell Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope CurrentUser
echo ### 执行应用安装脚本，请按提示操作 ###
powershell .\Install.ps1 
echo # 完成！按下任意键退出
pause > nul