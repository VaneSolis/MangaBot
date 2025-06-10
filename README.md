# MangaBot

API para gestionar una base de datos de mangas con más de 3,500 registros.

## Tecnologías Utilizadas

- .NET 9.0
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI

## Características

- Base de datos con 3,500 registros de mangas
- API RESTful
- Documentación con Swagger
- Datos aleatorios generados automáticamente

## Estructura de Datos

Cada manga contiene:
- Título
- Autor
- Fecha de Publicación
- Género
- Número de Capítulos

## Colaboradores

- [@VaneSolis](https://github.com/VaneSolis)

## Configuración

1. Clona el repositorio
2. Restaura los paquetes NuGet:
   ```bash
   dotnet restore
   ```
3. Actualiza la cadena de conexión en `appsettings.json`
4. Ejecuta las migraciones:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
5. Inicia la aplicación:
   ```bash
   dotnet run
   ```

## API Endpoints

La documentación completa de la API está disponible en Swagger UI cuando ejecutas la aplicación. 