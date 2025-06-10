FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MangaBot-1.sln", "./"]
COPY ["MangaBot.csproj", "./"]
RUN dotnet restore "MangaBot-1.sln"
COPY . .
RUN dotnet build "MangaBot-1.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MangaBot-1.sln" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangaBot.dll"] 