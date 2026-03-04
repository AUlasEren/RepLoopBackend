FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY AuthService/src/ AuthService/src/
RUN dotnet restore AuthService/src/RepLoopBackend.API/RepLoopBackend.API.csproj
RUN dotnet publish AuthService/src/RepLoopBackend.API/RepLoopBackend.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RepLoopBackend.API.dll"]
