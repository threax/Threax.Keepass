FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app

FROM threax/build-dotnet:3.0.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/KeePassWeb
RUN npm install
RUN dotnet restore KeePassWeb.csproj
RUN dotnet build KeePassWeb.csproj -c Release -o /app
RUN npm run clean
RUN npm run build

FROM build AS publish
RUN dotnet publish KeePassWeb.csproj -c Release -o /app

FROM base AS final
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KeePassWeb.dll"]
