FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY SessionService/src/ SessionService/src/
RUN dotnet restore SessionService/src/SessionService.API/SessionService.API.csproj
RUN dotnet publish SessionService/src/SessionService.API/SessionService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SessionService.API.dll"]
