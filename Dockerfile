# syntax=docker/dockerfile:1.7-labs

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY --exclude=*.cs src .
RUN dotnet restore Messenger/Presentation/Messenger.Presentation.csproj

COPY src .
WORKDIR /src/Messenger/Presentation
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS production

ARG API_VERSION=unversioned
ENV API_VERSION=$API_VERSION

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Messenger.Presentation.dll"]
