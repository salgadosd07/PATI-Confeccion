using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AsignacionesTallerController : ControllerBase
{
    private readonly IAsignacionTallerService _asignacionService;

    public AsignacionesTallerController(IAsignacionTallerService asignacionService)
    {
        _asignacionService = asignacionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AsignacionTallerDto>>> GetAll()
    {
        var asignaciones = await _asignacionService.GetAllAsignacionesAsync();
        return Ok(asignaciones);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AsignacionTallerDto>> GetById(int id)
    {
        var asignacion = await _asignacionService.GetAsignacionByIdAsync(id);
        if (asignacion == null)
            return NotFound();

        return Ok(asignacion);
    }

    [HttpGet("taller/{tallerId}")]
    public async Task<ActionResult<IEnumerable<AsignacionTallerDto>>> GetByTaller(int tallerId)
    {
        var asignaciones = await _asignacionService.GetAsignacionesByTallerAsync(tallerId);
        return Ok(asignaciones);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<AsignacionTallerDto>> Create([FromBody] CreateAsignacionTallerDto dto)
    {
        var asignacion = await _asignacionService.CreateAsignacionAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = asignacion.Id }, asignacion);
    }
}
