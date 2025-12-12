namespace PATI.Application.DTOs;

public class InventarioDto
{
    public int Id { get; set; }
    public int ReferenciaId { get; set; }
    public string? ReferenciaNombre { get; set; }
    public int TallaId { get; set; }
    public string? TallaNombre { get; set; }
    public int ColorId { get; set; }
    public string? ColorNombre { get; set; }
    public string? CodigoLote { get; set; }
    public int CantidadDisponible { get; set; }
    public int CantidadReservada { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public string? Ubicacion { get; set; }
    public string EstadoInventario { get; set; } = string.Empty;
}

public class CreateInventarioDto
{
    public int ReferenciaId { get; set; }
    public int TallaId { get; set; }
    public int ColorId { get; set; }
    public string? CodigoLote { get; set; }
    public int CantidadDisponible { get; set; }
    public string? Ubicacion { get; set; }
}
