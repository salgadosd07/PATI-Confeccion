namespace PATI.Application.DTOs;

public class RemisionDto
{
    public int Id { get; set; }
    public string NumeroRemision { get; set; } = string.Empty;
    public int AsignacionTallerId { get; set; }
    public string? TallerNombre { get; set; }
    public string? ReferenciaNombre { get; set; }
    public DateTime FechaDespacho { get; set; }
    public DateTime? FechaRecepcion { get; set; }
    public int CantidadEnviada { get; set; }
    public int? CantidadRecibida { get; set; }
    public string? RevisadoPor { get; set; }
    public string EstadoRemision { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public List<RemisionDetalleDto> Detalles { get; set; } = new();
}

public class RemisionDetalleDto
{
    public int TallaId { get; set; }
    public string? TallaNombre { get; set; }
    public int ColorId { get; set; }
    public string? ColorNombre { get; set; }
    public int Cantidad { get; set; }
}

public class CreateRemisionDto
{
    public int AsignacionTallerId { get; set; }
    public DateTime FechaDespacho { get; set; }
    public int CantidadEnviada { get; set; }
    public string? Observaciones { get; set; }
    public List<RemisionDetalleDto> Detalles { get; set; } = new();
}

public class RegistrarRecepcionDto
{
    public DateTime FechaRecepcion { get; set; }
    public int CantidadRecibida { get; set; }
    public string? RevisadoPor { get; set; }
    public string? Observaciones { get; set; }
}
