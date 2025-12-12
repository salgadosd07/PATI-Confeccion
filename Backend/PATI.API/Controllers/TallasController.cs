using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PATI.Domain.Entities;
using PATI.Infrastructure.Data;

namespace PATI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TallasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TallasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Talla>>> GetAll()
    {
        return Ok(await _context.Tallas.Where(t => t.Activo).OrderBy(t => t.Orden).ToListAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<Talla>> Create([FromBody] Talla talla)
    {
        talla.FechaCreacion = DateTime.UtcNow;
        _context.Tallas.Add(talla);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = talla.Id }, talla);
    }
}
