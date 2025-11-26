FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файлы проектов для restore
COPY ./Server/Api/Src/ ./Server/Api/Src/
COPY ./Server/Core/Src/ ./Server/Core/Src/
COPY ./Server/Infrastructure/Persistence/Src/ ./Server/Infrastructure/Persistence/Src/
COPY ./Shared/Contracts/Api/ ./Shared/Contracts/Api/

RUN dotnet restore "./Server/Api/Src/TDM.Server.Api/TDM.Server.Api.csproj"
RUN dotnet build "./Server/Api/Src/TDM.Server.Api/TDM.Server.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Server/Api/Src/TDM.Server.Api/TDM.Server.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TDM.Server.Api.dll"]