# syntax=docker/dockerfile:1.7
# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM yy556023/abp-sdk:10.0 AS build

# 設置 NUGET_XMLDOC_MODE 環境變數，以生成 XML 文檔
ENV NUGET_XMLDOC_MODE=none

WORKDIR /
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["common.props", "common.props"]
COPY ["NuGet.Config", "NuGet.Config"]
COPY ["src/Yuu.Eip.Application/Yuu.Eip.Application.csproj", "src/Yuu.Eip.Application/"]
COPY ["src/Yuu.Eip.Application.Contracts/Yuu.Eip.Application.Contracts.csproj", "src/Yuu.Eip.Application.Contracts/"]
COPY ["src/Yuu.Eip.Domain/Yuu.Eip.Domain.csproj", "src/Yuu.Eip.Domain/"]
COPY ["src/Yuu.Eip.Domain.Shared/Yuu.Eip.Domain.Shared.csproj", "src/Yuu.Eip.Domain.Shared/"]
COPY ["src/Yuu.Eip.EntityFrameworkCore/Yuu.Eip.EntityFrameworkCore.csproj", "src/Yuu.Eip.EntityFrameworkCore/"]
COPY ["src/Yuu.Eip.HttpApi/Yuu.Eip.HttpApi.csproj", "src/Yuu.Eip.HttpApi/"]
COPY ["src/Yuu.Eip.HttpApi.Client/Yuu.Eip.HttpApi.Client.csproj", "src/Yuu.Eip.HttpApi.Client/"]
COPY ["src/Yuu.Eip.HttpApi.Host/Yuu.Eip.HttpApi.Host.csproj", "src/Yuu.Eip.HttpApi.Host/"]

RUN --mount=type=secret,id=nuget_token \
    dotnet nuget update source "github" \
      --username yy556023 \
      --password "$(cat /run/secrets/nuget_token)" \
      --store-password-in-clear-text && \
    dotnet restore "src/Yuu.Eip.HttpApi.Host/Yuu.Eip.HttpApi.Host.csproj"

COPY . .
WORKDIR "/src/Yuu.Eip.HttpApi.Host"
RUN abp install-libs
RUN dotnet build "Yuu.Eip.HttpApi.Host.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yuu.Eip.HttpApi.Host.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password="36b1fcc7-309a-42fd-92c5-10afb4d39f10"
# ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/openiddict.pfx"

WORKDIR /app
# COPY ["application_default_credentials.json", "application_default_credentials.json"]
# COPY ["openiddict.pfx", "openiddict.pfx"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yuu.Eip.HttpApi.Host.dll"]
