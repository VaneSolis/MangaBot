using System.ComponentModel.DataAnnotations;

namespace MangaBot.Models;

public class Manga
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Autor { get; set; } = string.Empty;
    
    public DateTime FechaPublicacion { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Genero { get; set; } = string.Empty;
    
    public int Capitulos { get; set; }
} 