
set SolutionDir=".."

del "%cd%\YiShaAdmin\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.Web\YiSha.Admin.Web.csproj -c Debug -o "%cd%\YiShaAdmin"

del "%cd%\YiShaAdminApi\*.*" /f /s /q /a
dotnet publish %SolutionDir%\YiSha.Web\YiSha.Admin.WebApi\YiSha.Admin.WebApi.csproj -c Debug -o "%cd%\YiShaAdminApi"

pause