using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PATI.Domain.Entities;
using PATI.Infrastructure.Data;

namespace PATI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ColoresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ColoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Color>>> GetAll()
    {
        return Ok(await _context.Colores.Where(c => c.Activo).ToListAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<Color>> Create([FromBody] Color color)
    {
        color.FechaCreacion = DateTime.UtcNow;
        _context.Colores.Add(color);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = color.Id }, color);
    }
}
