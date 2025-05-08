FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Messenger/Options/*.csproj Messenger/Options/
COPY src/Messenger/Contracts/*.csproj Messenger/Contracts/
COPY src/Messenger/Core/*.csproj Messenger/Core/
COPY src/Messenger/ApiErrors/*.csproj Messenger/ApiErrors/
COPY src/Messenger/Data/Interfaces/*.csproj Messenger/Data/Interfaces/
COPY src/Messenger/Data/Scylla/*.csproj Messenger/Data/Implementations/
COPY src/Messenger/Gateway/*.csproj Messenger/Gateway/
COPY src/Messenger/Presentation/*.csproj Messenger/Presentation/
COPY src/Messenger/Infrastructure/*.csproj Messenger/Infrastructure/
COPY src/Messenger/Domain/*.csproj Messenger/Domain/
COPY src/Messenger/Application/*.csproj Messenger/Application/

RUN dotnet restore Messenger/Presentation/Messenger.Presentation.csproj

COPY src .
WORKDIR /src/Messenger/Presentation
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS production
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Messenger.Presentation.dll"]
