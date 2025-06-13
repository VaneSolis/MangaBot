FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MangaBot.csproj", "./"]
RUN dotnet restore "MangaBot.csproj"
COPY . .
RUN dotnet build "MangaBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MangaBot.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configuración para Railway
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Exponer el puerto por defecto
EXPOSE 8080

# Script de inicio que espera a que la base de datos esté lista
COPY <<EOF /app/entrypoint.sh
#!/bin/bash
echo "Esperando a que la base de datos esté lista..."
sleep 10
echo "Iniciando la aplicación..."
dotnet MangaBot.dll
EOF

RUN chmod +x /app/entrypoint.sh
ENTRYPOINT ["/app/entrypoint.sh"] 