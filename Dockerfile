FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MangaBot.csproj", "./"]
COPY ["Controllers/", "./Controllers/"]
COPY ["Models/", "./Models/"]
COPY ["Services/", "./Services/"]
COPY ["Program.cs", "./"]
RUN dotnet restore "MangaBot.csproj" --verbosity detailed
RUN dotnet build "MangaBot.csproj" -c Release -o /app/build --verbosity detailed

FROM build AS publish
RUN dotnet publish "MangaBot.csproj" -c Release -o /app/publish --verbosity detailed

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "MangaBot.dll"] 