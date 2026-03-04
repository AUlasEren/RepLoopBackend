FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY UserService/src/ UserService/src/
RUN dotnet restore UserService/src/UserService.API/UserService.API.csproj
RUN dotnet publish UserService/src/UserService.API/UserService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]
