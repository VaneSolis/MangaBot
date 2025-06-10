using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangaBot.Data;
using MangaBot.Models;

namespace MangaBot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MangaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MangaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetMangas()
    {
        return await _context.Mangas.ToListAsync();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetMangaCount()
    {
        return await _context.Mangas.CountAsync();
    }

    [HttpGet("random")]
    public async Task<ActionResult<Manga>> GetRandomManga()
    {
        var count = await _context.Mangas.CountAsync();
        if (count == 0)
            return NotFound("No hay mangas en la base de datos");

        var random = new Random();
        var skip = random.Next(0, count);
        var manga = await _context.Mangas.Skip(skip).FirstOrDefaultAsync();

        if (manga == null)
            return NotFound("No se pudo encontrar un manga aleatorio");

        return manga;
    }
} 