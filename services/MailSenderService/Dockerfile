FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY Shared/ Shared/
COPY MailSenderService/src/ MailSenderService/src/
RUN dotnet restore MailSenderService/src/MailSenderService/MailSenderService.csproj
RUN dotnet publish MailSenderService/src/MailSenderService/MailSenderService.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MailSenderService.dll"]
