namespace PATI.Domain.Entities;

public class AsignacionTaller : BaseEntity
{
    public string CodigoAsignacion { get; set; } = string.Empty;
    public int TallerId { get; set; }
    public int ReferenciaId { get; set; }
    public int CorteId { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public DateTime? FechaEstimadaEntrega { get; set; }
    public int CantidadAsignada { get; set; }
    public decimal? ValorUnitario { get; set; }
    public decimal? ValorTotal { get; set; }
    public string? Observaciones { get; set; }
    
    public Taller Taller { get; set; } = null!;
    public Referencia Referencia { get; set; } = null!;
    public Corte Corte { get; set; } = null!;
    public ICollection<AvanceTaller> Avances { get; set; } = new List<AvanceTaller>();
    public ICollection<Remision> Remisiones { get; set; } = new List<Remision>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
