FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY SettingsService/src/ SettingsService/src/
RUN dotnet restore SettingsService/src/SettingsService.API/SettingsService.API.csproj
RUN dotnet publish SettingsService/src/SettingsService.API/SettingsService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SettingsService.API.dll"]
