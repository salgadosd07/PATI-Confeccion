using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ControlCalidadController : ControllerBase
{
    private readonly IControlCalidadService _controlService;

    public ControlCalidadController(IControlCalidadService controlService)
    {
        _controlService = controlService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ControlCalidadDto>> GetById(int id)
    {
        var control = await _controlService.GetControlByIdAsync(id);
        if (control == null)
            return NotFound();

        return Ok(control);
    }

    [HttpGet("remision/{remisionId}")]
    public async Task<ActionResult<IEnumerable<ControlCalidadDto>>> GetByRemision(int remisionId)
    {
        var controles = await _controlService.GetControlesByRemisionAsync(remisionId);
        return Ok(controles);
    }

    [HttpPost]
    public async Task<ActionResult<ControlCalidadDto>> Create([FromBody] CreateControlCalidadDto dto)
    {
        var control = await _controlService.CreateControlAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = control.Id }, control);
    }

    [HttpPut("{id}/estado-arreglos")]
    public async Task<ActionResult<ControlCalidadDto>> ActualizarEstadoArreglos(int id, [FromBody] ActualizarEstadoArreglosDto dto)
    {
        var control = await _controlService.ActualizarEstadoArreglosAsync(id, dto.Estado);
        return Ok(control);
    }
}

public class ActualizarEstadoArreglosDto
{
    public string Estado { get; set; } = string.Empty;
}
