
set SolutionDir=".."

del "%cd%\*.7z" /f /s /q /a

del "%cd%\YiShaAdmin\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.Web\YiSha.Admin.Web.csproj -c Release -o "%cd%\YiShaAdmin"
del "%cd%\YiShaAdmin\appsettings.json" /f /s /q /a
del "%cd%\YiShaAdmin\web.config" /f /s /q /a
del "%cd%\YiShaAdmin\Resource\*.*" /f /s /q /a

del "%cd%\YiShaAdminApi\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.WebApi\YiSha.Admin.WebApi.csproj -c Release -o "%cd%\YiShaAdminApi"
del "%cd%\YiShaAdminApi\appsettings.json" /f /s /q /a
del "%cd%\YiShaAdminApi\web.config" /f /s /q /a
del "%cd%\YiShaAdminApi\Resource\*.*" /f /s /q /a

rem 7z
"C:\Program Files\7-Zip\7z.exe" a -t7z Publish.7z YiShaAdmin YiShaAdminApi

pause