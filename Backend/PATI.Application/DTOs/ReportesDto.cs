namespace PATI.Application.DTOs;

public class ReportePorReferenciaDto
{
    public int ReferenciaId { get; set; }
    public string? ReferenciaNombre { get; set; }
    public int CantidadTotalProgramada { get; set; }
    public int CantidadTotalCortada { get; set; }
    public int CantidadAsignada { get; set; }
    public int CantidadTerminada { get; set; }
    public int TotalImperfectos { get; set; }
    public int TotalArreglos { get; set; }
    public int TotalPendientes { get; set; }
    public int TotalAprobados { get; set; }
}

public class ReportePorTallerDto
{
    public int TallerId { get; set; }
    public string? TallerNombre { get; set; }
    public int TotalAsignaciones { get; set; }
    public int ProduccionTotal { get; set; }
    public int ProduccionTerminada { get; set; }
    public decimal RendimientoPromedio { get; set; }
    public double TiempoPromedioEntrega { get; set; }
    public int ImperfectosRecurrentes { get; set; }
    public decimal TotalPagado { get; set; }
}

public class ReporteFinancieroDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal TotalPagosProgramados { get; set; }
    public decimal TotalPagosPagados { get; set; }
    public decimal TotalPagosPendientes { get; set; }
    public decimal TotalPagosParciales { get; set; }
    public decimal CostoPromedioUnidad { get; set; }
    public List<PagoTallerDto> PagosPorTaller { get; set; } = new();
}

public class PagoTallerDto
{
    public int TallerId { get; set; }
    public string TallerNombre { get; set; } = string.Empty;
    public decimal TotalPagado { get; set; }
    public decimal TotalPendiente { get; set; }
}

public class ReporteColorDto
{
    public int ColorId { get; set; }
    public string? ColorNombre { get; set; }
    public int CantidadTotal { get; set; }
    public int CantidadDisponible { get; set; }
    public List<string> ReferenciasUsadas { get; set; } = new();
}
