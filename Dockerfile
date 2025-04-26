FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/MessengerAPI/Contracts/*.csproj MessengerAPI/Contracts/
COPY src/MessengerAPI/Core/*.csproj MessengerAPI/Core/
COPY src/MessengerAPI/Errors/*.csproj MessengerAPI/Errors/
COPY src/MessengerAPI/Data.Interfaces/*.csproj MessengerAPI/Data/Interfaces/
COPY src/MessengerAPI/Data.Implementations/*.csproj MessengerAPI/Data/Implementations/
COPY src/MessengerAPI/Gateway/*.csproj MessengerAPI/Gateway/
COPY src/MessengerAPI/Presentation/*.csproj MessengerAPI/Presentation/
COPY src/MessengerAPI/Infrastructure/*.csproj MessengerAPI/Infrastructure/
COPY src/MessengerAPI/Domain/*.csproj MessengerAPI/Domain/
COPY src/MessengerAPI/Application/*.csproj MessengerAPI/Application/

RUN dotnet restore MessengerAPI/Presentation/MessengerAPI.Presentation.csproj

COPY src .
WORKDIR /src/MessengerAPI/Presentation
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS production
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MessengerAPI.Presentation.dll"]
