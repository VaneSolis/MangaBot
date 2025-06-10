using Bogus;
using MangaBot.Models;

namespace MangaBot.Services
{
    public class MangaFakerService
    {
        private readonly Faker<Manga> _mangaFaker;
        private readonly HashSet<string> _titulosUnicos;

        public MangaFakerService()
        {
            _titulosUnicos = new HashSet<string>();
            
            _mangaFaker = new Faker<Manga>()
                .RuleFor(m => m.Titulo, f => GenerateUniqueTitle(f))
                .RuleFor(m => m.Autor, f => f.Name.FullName())
                .RuleFor(m => m.Genero, f => f.PickRandom(new[] { "Acción", "Aventura", "Comedia", "Drama", "Fantasía", "Horror", "Misterio", "Romance", "Ciencia Ficción", "Slice of Life" }))
                .RuleFor(m => m.Volumenes, f => f.Random.Number(1, 50))
                .RuleFor(m => m.FechaPublicacion, f => f.Date.Past(10))
                .RuleFor(m => m.Descripcion, f => f.Lorem.Paragraph())
                .RuleFor(m => m.Estado, f => f.Random.Bool());
        }

        private string GenerateUniqueTitle(Faker f)
        {
            string title;
            do
            {
                title = $"{f.Random.Word()} {f.Random.Word()} {f.Random.Word()}";
            } while (!_titulosUnicos.Add(title));

            return title;
        }

        public IEnumerable<Manga> GenerateMangas(int count)
        {
            return _mangaFaker.Generate(count);
        }
    }
} 