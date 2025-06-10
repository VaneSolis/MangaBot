using Microsoft.EntityFrameworkCore;
using MangaBot.Data;
using MangaBot.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar la base de datos
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar el servicio de generación de datos
builder.Services.AddScoped<MangaFakerService>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Asegurarse de que la base de datos existe y está actualizada
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        // Generar datos si la base de datos está vacía
        if (!context.Mangas.Any())
        {
            var fakerService = services.GetRequiredService<MangaFakerService>();
            var mangas = fakerService.GenerateMangas(3500);
            context.Mangas.AddRange(mangas);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar la base de datos.");
    }
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Endpoint raíz
app.MapGet("/", () => "MangaBot API está funcionando correctamente!");

// Endpoint de health check para Railway
app.MapGet("/health", () => Results.Ok());

// Mapear controladores
app.MapControllers();

app.Run();
