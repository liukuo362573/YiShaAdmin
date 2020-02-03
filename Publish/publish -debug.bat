
set SolutionDir=".."

del "%cd%\Admin\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.Web\YiSha.Admin.Web.csproj -c Debug -o "%cd%\Admin"

del "%cd%\Api\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.WebApi\YiSha.Admin.WebApi.csproj -c Debug -o "%cd%\Api"

pause