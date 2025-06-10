using Microsoft.EntityFrameworkCore;
using MangaBot.Models;

namespace MangaBot.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Manga> Mangas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Manga>()
            .HasIndex(m => m.Titulo)
            .IsUnique();
    }
} 