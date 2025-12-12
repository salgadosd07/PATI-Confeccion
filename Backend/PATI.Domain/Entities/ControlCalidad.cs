namespace PATI.Domain.Entities;

public class ControlCalidad : BaseEntity
{
    public int RemisionId { get; set; }
    public DateTime FechaControl { get; set; }
    public int CantidadImperfectos { get; set; }
    public int CantidadArreglos { get; set; }
    public int CantidadPendientes { get; set; }
    public int CantidadAprobados { get; set; }
    public string? CausaImperfecto { get; set; }
    public string? Observaciones { get; set; }
    public string EstadoArreglos { get; set; } = "Pendiente";
    public string? RevisadoPor { get; set; }
    
    public Remision Remision { get; set; } = null!;
    public ICollection<DetalleImperfecto> DetallesImperfectos { get; set; } = new List<DetalleImperfecto>();
}
