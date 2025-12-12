namespace PATI.Domain.Entities;

public class Pago : BaseEntity
{
    public string NumeroPago { get; set; } = string.Empty;
    public int AsignacionTallerId { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal? MontoPagado { get; set; }
    public string EstadoPago { get; set; } = "Pendiente";
    public string? MetodoPago { get; set; }
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
    public int? DiasMora { get; set; }
    
    public AsignacionTaller AsignacionTaller { get; set; } = null!;
}
