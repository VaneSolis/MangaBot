using Microsoft.EntityFrameworkCore;
using MangaBot.Models;

namespace MangaBot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Manga> Mangas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar índice único para el título
            modelBuilder.Entity<Manga>()
                .HasIndex(m => m.Titulo)
                .IsUnique();

            // Configurar índices para mejorar el rendimiento de búsqueda
            modelBuilder.Entity<Manga>()
                .HasIndex(m => m.Genero);

            modelBuilder.Entity<Manga>()
                .HasIndex(m => m.Autor);

            modelBuilder.Entity<Manga>()
                .HasIndex(m => m.FechaPublicacion);
        }
    }
} 