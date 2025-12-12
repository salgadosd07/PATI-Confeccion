using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventarioController : ControllerBase
{
    private readonly IInventarioService _inventarioService;

    public InventarioController(IInventarioService inventarioService)
    {
        _inventarioService = inventarioService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InventarioDto>> GetById(int id)
    {
        var inventario = await _inventarioService.GetInventarioByIdAsync(id);
        if (inventario == null)
            return NotFound();

        return Ok(inventario);
    }

    [HttpGet("disponible")]
    public async Task<ActionResult<IEnumerable<InventarioDto>>> GetDisponible()
    {
        var inventarios = await _inventarioService.GetInventarioDisponibleAsync();
        return Ok(inventarios);
    }

    [HttpGet("referencia/{referenciaId}")]
    public async Task<ActionResult<IEnumerable<InventarioDto>>> GetByReferencia(int referenciaId)
    {
        var inventarios = await _inventarioService.GetInventarioByReferenciaAsync(referenciaId);
        return Ok(inventarios);
    }

    [HttpPost]
    public async Task<ActionResult<InventarioDto>> Create([FromBody] CreateInventarioDto dto)
    {
        var inventario = await _inventarioService.AgregarInventarioAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = inventario.Id }, inventario);
    }

    [HttpPost("actualizar-desde-remision/{remisionId}")]
    public async Task<ActionResult> ActualizarDesdeRemision(int remisionId)
    {
        await _inventarioService.ActualizarInventarioDesdeRemisionAsync(remisionId);
        return Ok(new { message = "Inventario actualizado correctamente" });
    }
}
