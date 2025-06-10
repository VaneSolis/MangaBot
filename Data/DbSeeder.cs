using Microsoft.EntityFrameworkCore;
using MangaBot.Models;

namespace MangaBot.Data;

public static class DbSeeder
{
    private static readonly string[] Generos = new[]
    {
        "Acción", "Aventura", "Comedia", "Drama", "Fantasía",
        "Horror", "Misterio", "Romance", "Ciencia Ficción", "Slice of Life"
    };

    private static readonly string[] Autores = new[]
    {
        "Eiichiro Oda", "Hirohiko Araki", "Tite Kubo", "Masashi Kishimoto",
        "Hajime Isayama", "Yoshihiro Togashi", "Kentaro Miura", "Naoki Urasawa",
        "Takehiko Inoue", "Akira Toriyama"
    };

    public static async Task SeedAsync(ApplicationDbContext context, int count)
    {
        if (await context.Mangas.AnyAsync())
        {
            return; // Database already seeded
        }

        var random = new Random();
        var mangas = new List<Manga>();

        for (int i = 0; i < count; i++)
        {
            var manga = new Manga
            {
                Titulo = $"Manga {i + 1}",
                Autor = Autores[random.Next(Autores.Length)],
                FechaPublicacion = DateTime.Now.AddYears(-random.Next(1, 20)),
                Genero = Generos[random.Next(Generos.Length)],
                Volumenes = random.Next(1, 50),
                Descripcion = "Descripción de ejemplo",
                Estado = random.Next(2) == 0
            };
            mangas.Add(manga);
        }

        await context.Mangas.AddRangeAsync(mangas);
        await context.SaveChangesAsync();
    }
} 