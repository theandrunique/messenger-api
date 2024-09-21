FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY MessengerAPI.Presentation/*.csproj MessengerAPI.Presentation/
COPY MessengerAPI.Infrastructure/*.csproj MessengerAPI.Infrastructure/
COPY MessengerAPI.Domain/*.csproj MessengerAPI.Domain/
COPY MessengerAPI.Application/*.csproj MessengerAPI.Application/

RUN dotnet restore MessengerAPI.Presentation/MessengerAPI.Presentation.csproj

COPY . .
WORKDIR "/src/MessengerAPI.Presentation"
RUN dotnet publish "./MessengerAPI.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS production
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MessengerAPI.Presentation.dll"]
