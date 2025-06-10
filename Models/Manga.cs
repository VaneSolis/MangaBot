using System.ComponentModel.DataAnnotations;

namespace MangaBot.Models;

public class Manga
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Autor { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Genero { get; set; } = string.Empty;
    
    public int Volumenes { get; set; }
    
    public DateTime FechaPublicacion { get; set; }
    
    [StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;
    
    public bool Estado { get; set; } // true = en publicación, false = finalizado
} 