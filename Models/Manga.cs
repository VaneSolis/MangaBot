using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaBot.Models;

public class Manga
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    [Column(TypeName = "nvarchar(200)")]
    public string Titulo { get; set; } = string.Empty;
    
    [StringLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string Autor { get; set; } = string.Empty;
    
    [StringLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public string Genero { get; set; } = string.Empty;
    
    public int Volumenes { get; set; }
    
    [Column(TypeName = "datetime2")]
    public DateTime FechaPublicacion { get; set; }
    
    [StringLength(500)]
    [Column(TypeName = "nvarchar(500)")]
    public string Descripcion { get; set; } = string.Empty;
    
    public bool Estado { get; set; } // true = en publicación, false = finalizado
} 