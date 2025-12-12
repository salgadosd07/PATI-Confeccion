using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PATI.Domain.Entities;
using PATI.Infrastructure.Data;

namespace PATI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TalleresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TalleresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Taller>>> GetAll()
    {
        return Ok(await _context.Talleres.Where(t => t.Activo).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Taller>> GetById(int id)
    {
        var taller = await _context.Talleres.FindAsync(id);
        if (taller == null)
            return NotFound();

        return Ok(taller);
    }

    [HttpPost]
    public async Task<ActionResult<Taller>> Create([FromBody] Taller taller)
    {
        taller.FechaCreacion = DateTime.UtcNow;
        _context.Talleres.Add(taller);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = taller.Id }, taller);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Taller taller)
    {
        if (id != taller.Id)
            return BadRequest();

        taller.FechaModificacion = DateTime.UtcNow;
        _context.Entry(taller).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var taller = await _context.Talleres.FindAsync(id);
        if (taller == null)
            return NotFound();

        taller.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
