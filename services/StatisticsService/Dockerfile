FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY StatisticsService/src/ StatisticsService/src/
RUN dotnet restore StatisticsService/src/StatisticsService.API/StatisticsService.API.csproj
RUN dotnet publish StatisticsService/src/StatisticsService.API/StatisticsService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StatisticsService.API.dll"]