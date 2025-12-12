using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagosController : ControllerBase
{
    private readonly IPagoService _pagoService;

    public PagosController(IPagoService pagoService)
    {
        _pagoService = pagoService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PagoDto>> GetById(int id)
    {
        var pago = await _pagoService.GetPagoByIdAsync(id);
        if (pago == null)
            return NotFound();

        return Ok(pago);
    }

    [HttpGet("asignacion/{asignacionId}")]
    public async Task<ActionResult<IEnumerable<PagoDto>>> GetByAsignacion(int asignacionId)
    {
        var pagos = await _pagoService.GetPagosByAsignacionAsync(asignacionId);
        return Ok(pagos);
    }

    [HttpGet("pendientes")]
    public async Task<ActionResult<IEnumerable<PagoDto>>> GetPendientes()
    {
        var pagos = await _pagoService.GetPagosPendientesAsync();
        return Ok(pagos);
    }

    [HttpGet("taller/{tallerId}/total")]
    public async Task<ActionResult<decimal>> GetTotalPagadoByTaller(int tallerId)
    {
        var total = await _pagoService.CalcularTotalPagadoByTallerAsync(tallerId);
        return Ok(new { tallerId, totalPagado = total });
    }

    [HttpPost]
    public async Task<ActionResult<PagoDto>> Create([FromBody] CreatePagoDto dto)
    {
        var pago = await _pagoService.CreatePagoAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = pago.Id }, pago);
    }

    [HttpPut("{id}/estado")]
    public async Task<ActionResult<PagoDto>> ActualizarEstado(int id, [FromBody] ActualizarEstadoPagoDto dto)
    {
        var pago = await _pagoService.ActualizarEstadoPagoAsync(id, dto);
        return Ok(pago);
    }
}
