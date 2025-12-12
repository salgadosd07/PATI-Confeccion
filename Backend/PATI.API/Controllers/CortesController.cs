using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CortesController : ControllerBase
{
    private readonly ICorteService _corteService;

    public CortesController(ICorteService corteService)
    {
        _corteService = corteService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Corte,Taller,Calidad,Bodega")]
    public async Task<ActionResult<IEnumerable<CorteDto>>> GetAll()
    {
        var cortes = await _corteService.GetAllCortesAsync();
        return Ok(cortes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CorteDto>> GetById(int id)
    {
        var corte = await _corteService.GetCorteByIdAsync(id);
        if (corte == null)
            return NotFound();

        return Ok(corte);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Corte")]
    public async Task<ActionResult<CorteDto>> Create([FromBody] CreateCorteDto dto)
    {
        var corte = await _corteService.CreateCorteAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = corte.Id }, corte);
    }
}
