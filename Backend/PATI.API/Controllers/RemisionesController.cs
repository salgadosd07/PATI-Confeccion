using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RemisionesController : ControllerBase
{
    private readonly IRemisionService _remisionService;

    public RemisionesController(IRemisionService remisionService)
    {
        _remisionService = remisionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RemisionDto>>> GetAll()
    {
        var remisiones = await _remisionService.GetAllRemisionesAsync();
        return Ok(remisiones);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RemisionDto>> GetById(int id)
    {
        var remision = await _remisionService.GetRemisionByIdAsync(id);
        if (remision == null)
            return NotFound();

        return Ok(remision);
    }

    [HttpGet("asignacion/{asignacionId}")]
    public async Task<ActionResult<IEnumerable<RemisionDto>>> GetByAsignacion(int asignacionId)
    {
        var remisiones = await _remisionService.GetRemisionesByAsignacionAsync(asignacionId);
        return Ok(remisiones);
    }

    [HttpPost]
    public async Task<ActionResult<RemisionDto>> Create([FromBody] CreateRemisionDto dto)
    {
        var remision = await _remisionService.CreateRemisionAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = remision.Id }, remision);
    }

    [HttpPut("{id}/recepcion")]
    public async Task<ActionResult<RemisionDto>> RegistrarRecepcion(int id, [FromBody] RegistrarRecepcionDto dto)
    {
        var remision = await _remisionService.RegistrarRecepcionAsync(id, dto);
        return Ok(remision);
    }
}
