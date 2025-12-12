using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PATI.Domain.Entities;
using PATI.Infrastructure.Data;

namespace PATI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MaterialesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MaterialesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Material>>> GetAll()
    {
        return Ok(await _context.Materiales.Where(m => m.Activo).ToListAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Corte,Bodega")]
    public async Task<ActionResult<Material>> Create([FromBody] Material material)
    {
        material.FechaCreacion = DateTime.UtcNow;
        _context.Materiales.Add(material);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = material.Id }, material);
    }
}
