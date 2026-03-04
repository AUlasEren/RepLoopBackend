FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY WorkoutService/src/ WorkoutService/src/
RUN dotnet restore WorkoutService/src/WorkoutService.API/WorkoutService.API.csproj
RUN dotnet publish WorkoutService/src/WorkoutService.API/WorkoutService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WorkoutService.API.dll"]
