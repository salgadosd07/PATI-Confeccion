using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PATI.Domain.Entities;
using PATI.Infrastructure.Data;

namespace PATI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReferenciasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReferenciasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Referencia>>> GetAll()
    {
        return Ok(await _context.Referencias.Where(r => r.Activo).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Referencia>> GetById(int id)
    {
        var referencia = await _context.Referencias.FindAsync(id);
        if (referencia == null)
            return NotFound();

        return Ok(referencia);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Corte")]
    public async Task<ActionResult<Referencia>> Create([FromBody] Referencia referencia)
    {
        referencia.FechaCreacion = DateTime.UtcNow;
        _context.Referencias.Add(referencia);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = referencia.Id }, referencia);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Referencia referencia)
    {
        if (id != referencia.Id)
            return BadRequest();

        referencia.FechaModificacion = DateTime.UtcNow;
        _context.Entry(referencia).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var referencia = await _context.Referencias.FindAsync(id);
        if (referencia == null)
            return NotFound();

        referencia.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
