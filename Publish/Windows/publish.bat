
C:\Windows\System32\inetsrv\appcmd.exe stop apppool /apppool.name:"YiShaAdmin" 
C:\Windows\System32\inetsrv\appcmd.exe stop apppool /apppool.name:"YiShaApi" 

del %cd%\Admin\*.dll /f /s /q /a
del %cd%\Admin\*.pdb /f /s /q /a
del %cd%\Api\*.dll /f /s /q /a
del %cd%\Api\*.pdb /f /s /q /a

xcopy %cd%\Publish\Admin     %cd%\Admin     /s /e /y
xcopy %cd%\Publish\Api  %cd%\Api  /s /e /y

C:\Windows\System32\inetsrv\appcmd.exe start apppool /apppool.name:"YiShaAdmin" 
C:\Windows\System32\inetsrv\appcmd.exe start apppool /apppool.name:"YiShaApi" 

pause