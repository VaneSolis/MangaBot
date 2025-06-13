using Microsoft.EntityFrameworkCore;
using MangaBot.Data;
using MangaBot.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cultura por defecto
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

// Add services to the container.
builder.Services.AddControllers();

// Configurar la base de datos
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

// Asegurarse de que la cadena de conexión no sea nula
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró la cadena de conexión a la base de datos. Por favor, configure la variable de entorno DATABASE_URL.");
}

// Log de la cadena de conexión (sin credenciales sensibles)
var safeConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions => 
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

// Registrar el servicio de generación de datos
builder.Services.AddScoped<MangaFakerService>();

// Configurar HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar el pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Configurar HTTPS
app.UseHttpsRedirection();

// Configurar CORS
app.UseCors("AllowAll");

// Endpoint de health check mejorado
app.MapGet("/health", async (ApplicationDbContext dbContext, ILogger<Program> logger) =>
{
    try
    {
        // Verificar la conexión a la base de datos
        if (await dbContext.Database.CanConnectAsync())
        {
            return Results.Ok(new { status = "healthy", database = "connected" });
        }
        return Results.Problem(detail: "Base de datos desconectada", statusCode: 503, title: "unhealthy");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error en health check");
        return Results.Problem(detail: ex.Message, statusCode: 503, title: "unhealthy");
    }
});

// Asegurarse de que la base de datos existe y está actualizada
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Intentando conectar a la base de datos...");
        await context.Database.EnsureCreatedAsync();
        logger.LogInformation("Base de datos creada/verificada exitosamente");

        // Generar datos si la base de datos está vacía
        if (!context.Mangas.Any())
        {
            logger.LogInformation("Generando datos iniciales...");
            var fakerService = services.GetRequiredService<MangaFakerService>();
            var mangas = fakerService.GenerateMangas(3500);
            await context.Mangas.AddRangeAsync(mangas);
            await context.SaveChangesAsync();
            logger.LogInformation("Datos iniciales generados exitosamente");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos: {Message}", ex.Message);
        throw; // Re-lanzar la excepción para que Railway sepa que hay un error
    }
}

app.UseAuthorization();

// Endpoint raíz
app.MapGet("/", () => "MangaBot API está funcionando correctamente!");

// Mapear controladores
app.MapControllers();

app.Run();
