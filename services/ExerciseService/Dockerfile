FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY ExerciseService/src/ ExerciseService/src/
RUN dotnet restore ExerciseService/src/ExerciseService.API/ExerciseService.API.csproj
RUN dotnet publish ExerciseService/src/ExerciseService.API/ExerciseService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExerciseService.API.dll"]
