FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY ["MangaBot.csproj", "./"]
RUN dotnet restore "MangaBot.csproj"

# Copiar el resto de los archivos
COPY . .
RUN dotnet build "MangaBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MangaBot.csproj" -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangaBot.dll"] 