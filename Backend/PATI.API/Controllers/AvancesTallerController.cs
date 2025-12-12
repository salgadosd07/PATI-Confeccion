using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvancesTallerController : ControllerBase
{
    private readonly IAvanceTallerService _avanceService;

    public AvancesTallerController(IAvanceTallerService avanceService)
    {
        _avanceService = avanceService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AvanceTallerDto>> GetById(int id)
    {
        var avance = await _avanceService.GetAvanceByIdAsync(id);
        if (avance == null)
            return NotFound();

        return Ok(avance);
    }

    [HttpGet("asignacion/{asignacionId}")]
    public async Task<ActionResult<IEnumerable<AvanceTallerDto>>> GetByAsignacion(int asignacionId)
    {
        var avances = await _avanceService.GetAvancesByAsignacionAsync(asignacionId);
        return Ok(avances);
    }

    [HttpGet("asignacion/{asignacionId}/ultimo")]
    public async Task<ActionResult<AvanceTallerDto>> GetUltimoAvance(int asignacionId)
    {
        var avance = await _avanceService.GetUltimoAvanceAsync(asignacionId);
        if (avance == null)
            return NotFound();

        return Ok(avance);
    }

    [HttpPost]
    public async Task<ActionResult<AvanceTallerDto>> Create([FromBody] CreateAvanceTallerDto dto)
    {
        var avance = await _avanceService.CreateAvanceAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = avance.Id }, avance);
    }
}
