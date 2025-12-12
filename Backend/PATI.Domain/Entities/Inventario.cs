namespace PATI.Domain.Entities;

public class Inventario : BaseEntity
{
    public int ReferenciaId { get; set; }
    public int TallaId { get; set; }
    public int ColorId { get; set; }
    public string? CodigoLote { get; set; }
    public int CantidadDisponible { get; set; }
    public int CantidadReservada { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public string? Ubicacion { get; set; }
    public string EstadoInventario { get; set; } = "Disponible";
    
    public Referencia Referencia { get; set; } = null!;
    public Talla Talla { get; set; } = null!;
    public Color Color { get; set; } = null!;
}
