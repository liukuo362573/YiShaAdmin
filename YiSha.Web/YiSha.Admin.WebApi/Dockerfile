#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM  mcr.microsoft.com/dotnet/aspnet:3.1 AS base
COPY . /app
WORKDIR /app
EXPOSE 5001

ENV TZ=Asia/Shanghai

CMD ["nohup","dotnet", "YiSha.Admin.WebApi.dll"]