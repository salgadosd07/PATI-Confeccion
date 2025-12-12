namespace PATI.Application.DTOs;

public class PagoDto
{
    public int Id { get; set; }
    public string NumeroPago { get; set; } = string.Empty;
    public int AsignacionTallerId { get; set; }
    public string? TallerNombre { get; set; }
    public string? ReferenciaNombre { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal? MontoPagado { get; set; }
    public string EstadoPago { get; set; } = string.Empty;
    public string? MetodoPago { get; set; }
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
    public int? DiasMora { get; set; }
}

public class CreatePagoDto
{
    public int AsignacionTallerId { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal? MontoPagado { get; set; }
    public string? MetodoPago { get; set; }
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
}

public class ActualizarEstadoPagoDto
{
    public decimal? MontoPagado { get; set; }
    public string EstadoPago { get; set; } = string.Empty;
    public string? MetodoPago { get; set; }
    public string? Observaciones { get; set; }
}
