namespace PATI.Application.DTOs;

public class AsignacionTallerDto
{
    public int Id { get; set; }
    public string CodigoAsignacion { get; set; } = string.Empty;
    public int TallerId { get; set; }
    public string? TallerNombre { get; set; }
    public int ReferenciaId { get; set; }
    public string? ReferenciaNombre { get; set; }
    public int CorteId { get; set; }
    public string? CodigoLote { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public DateTime? FechaEstimadaEntrega { get; set; }
    public int CantidadAsignada { get; set; }
    public decimal? ValorUnitario { get; set; }
    public decimal? ValorTotal { get; set; }
    public string? Observaciones { get; set; }
    public decimal? PorcentajeAvance { get; set; }
}

public class CreateAsignacionTallerDto
{
    public int TallerId { get; set; }
    public int ReferenciaId { get; set; }
    public int CorteId { get; set; }
    public DateTime FechaAsignacion { get; set; }
    public DateTime? FechaEstimadaEntrega { get; set; }
    public int CantidadAsignada { get; set; }
    public decimal? ValorUnitario { get; set; }
    public string? Observaciones { get; set; }
}
