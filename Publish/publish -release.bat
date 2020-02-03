
set SolutionDir=".."

del "%cd%\*.7z" /f /s /q /a

del "%cd%\Admin\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.Web\YiSha.Admin.Web.csproj -c Release -o "%cd%\Admin"
del "%cd%\Admin\appsettings.json" /f /s /q /a
del "%cd%\Admin\web.config" /f /s /q /a
del "%cd%\Admin\Resource\*.*" /f /s /q /a

del "%cd%\Api\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.WebApi\YiSha.Admin.WebApi.csproj -c Release -o "%cd%\Api"
del "%cd%\Api\appsettings.json" /f /s /q /a
del "%cd%\Api\web.config" /f /s /q /a
del "%cd%\Api\Resource\*.*" /f /s /q /a

rem 7z
"D:\Program Files\7-Zip\7z.exe" a -t7z Publish.7z Admin Api

pause