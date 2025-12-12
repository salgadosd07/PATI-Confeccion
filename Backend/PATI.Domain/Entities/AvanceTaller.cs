namespace PATI.Domain.Entities;

public class AvanceTaller : BaseEntity
{
    public int AsignacionTallerId { get; set; }
    public DateTime FechaReporte { get; set; }
    public int CantidadLista { get; set; }
    public int CantidadEnProceso { get; set; }
    public int CantidadPendiente { get; set; }
    public int CantidadDespachada { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public string? Observaciones { get; set; }
    public string? UrlFotoEvidencia { get; set; }
    
    public AsignacionTaller AsignacionTaller { get; set; } = null!;
}
