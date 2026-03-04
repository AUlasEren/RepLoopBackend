FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY ApiGateway/src/ApiGateway/ApiGateway.csproj ApiGateway/src/ApiGateway/
RUN dotnet restore ApiGateway/src/ApiGateway/ApiGateway.csproj
COPY ApiGateway/ ApiGateway/
RUN dotnet publish ApiGateway/src/ApiGateway/ApiGateway.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
