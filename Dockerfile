# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["InnoClinic.Notification.API/InnoClinic.Notification.API.csproj", "InnoClinic.Notification.API/"]
COPY ["InnoClinic.Notification.Application/InnoClinic.Notification.Application.csproj", "InnoClinic.Notification.Application/"]
COPY ["InnoClinic.Notification.Core/InnoClinic.Notification.Core.csproj", "InnoClinic.Notification.Core/"]
COPY ["InnoClinic.Notification.Infrastructure/InnoClinic.Notification.Infrastructure.csproj", "InnoClinic.Notification.Infrastructure/"]

# Копируем файл privateKey.xml
COPY ["privateKey.xml", "InnoClinic.Notification.API/"]

RUN dotnet restore "InnoClinic.Notification.API/InnoClinic.Notification.API.csproj"

COPY . .

WORKDIR "/src/InnoClinic.Notification.API"
RUN dotnet build "InnoClinic.Notification.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "InnoClinic.Notification.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Копируем файл privateKey.xml в финальный образ
COPY --from=build /src/InnoClinic.Notification.API/privateKey.xml .

ENTRYPOINT ["dotnet", "InnoClinic.Notification.API.dll"]