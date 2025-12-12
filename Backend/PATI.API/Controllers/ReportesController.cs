using Microsoft.AspNetCore.Mvc;
using PATI.Application.DTOs;
using PATI.Application.Services;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly IReportesService _reportesService;

    public ReportesController(IReportesService reportesService)
    {
        _reportesService = reportesService;
    }

    [HttpGet("referencia/{referenciaId}")]
    public async Task<ActionResult<ReportePorReferenciaDto>> GetReportePorReferencia(int referenciaId)
    {
        var reporte = await _reportesService.GetReportePorReferenciaAsync(referenciaId);
        return Ok(reporte);
    }

    [HttpGet("taller/{tallerId}")]
    public async Task<ActionResult<ReportePorTallerDto>> GetReportePorTaller(int tallerId)
    {
        var reporte = await _reportesService.GetReportePorTallerAsync(tallerId);
        return Ok(reporte);
    }

    [HttpGet("financiero")]
    public async Task<ActionResult<ReporteFinancieroDto>> GetReporteFinanciero(
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin)
    {
        var reporte = await _reportesService.GetReporteFinancieroAsync(fechaInicio, fechaFin);
        return Ok(reporte);
    }

    [HttpGet("colores")]
    public async Task<ActionResult<IEnumerable<ReporteColorDto>>> GetReporteColores()
    {
        var reporte = await _reportesService.GetReporteColoresAsync();
        return Ok(reporte);
    }
}
