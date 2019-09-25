
C:\Windows\System32\inetsrv\appcmd.exe stop apppool /apppool.name:"YiShaAdmin" 
C:\Windows\System32\inetsrv\appcmd.exe stop apppool /apppool.name:"YiShaAdminApi" 

del %cd%\YiShaAdmin\*.dll /f /s /q /a
del %cd%\YiShaAdmin\*.pdb /f /s /q /a
del %cd%\YiShaAdminApi\*.dll /f /s /q /a
del %cd%\YiShaAdminApi\*.pdb /f /s /q /a

xcopy %cd%\Publish\YiShaAdmin     %cd%\YiShaAdmin     /s /e /y
xcopy %cd%\Publish\YiShaAdminApi  %cd%\YiShaAdminApi  /s /e /y

C:\Windows\System32\inetsrv\appcmd.exe start apppool /apppool.name:"YiShaAdmin" 
C:\Windows\System32\inetsrv\appcmd.exe start apppool /apppool.name:"YiShaAdminApi" 

pause