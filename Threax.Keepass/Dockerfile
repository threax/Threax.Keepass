FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM threax/build-dotnet:6.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/Threax.Keepass
RUN --mount=type=cache,target=node_modules,id=threax-build-keepass npm install
RUN --mount=type=cache,target=/root/.nuget/packages,id=threax-build-dotnet-nuget dotnet restore Threax.Keepass.csproj
RUN --mount=type=cache,target=/root/.nuget/packages,id=threax-build-dotnet-nuget dotnet build Threax.Keepass.csproj -c Release -o /app
RUN --mount=type=cache,target=node_modules,id=threax-build-keepass npm run clean
RUN --mount=type=cache,target=node_modules,id=threax-build-keepass npm run build

FROM build AS publish
RUN --mount=type=cache,target=/root/.nuget/packages,id=threax-build-dotnet-nuget dotnet publish Threax.Keepass.csproj -c Release -o /app

FROM base AS final
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Threax.Keepass.dll"]
