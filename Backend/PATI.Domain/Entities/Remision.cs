namespace PATI.Domain.Entities;

public class Remision : BaseEntity
{
    public string NumeroRemision { get; set; } = string.Empty;
    public int AsignacionTallerId { get; set; }
    public DateTime FechaDespacho { get; set; }
    public DateTime? FechaRecepcion { get; set; }
    public int CantidadEnviada { get; set; }
    public int? CantidadRecibida { get; set; }
    public string? RevisadoPor { get; set; }
    public string EstadoRemision { get; set; } = "Pendiente";
    public string? Observaciones { get; set; }
    
    public AsignacionTaller AsignacionTaller { get; set; } = null!;
    public ICollection<RemisionDetalle> Detalles { get; set; } = new List<RemisionDetalle>();
    public ICollection<ControlCalidad> ControlesCalidad { get; set; } = new List<ControlCalidad>();
}
