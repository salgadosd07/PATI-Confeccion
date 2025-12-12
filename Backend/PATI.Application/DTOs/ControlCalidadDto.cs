namespace PATI.Application.DTOs;

public class ControlCalidadDto
{
    public int Id { get; set; }
    public int RemisionId { get; set; }
    public string? NumeroRemision { get; set; }
    public string? TallerNombre { get; set; }
    public DateTime FechaControl { get; set; }
    public int CantidadImperfectos { get; set; }
    public int CantidadArreglos { get; set; }
    public int CantidadPendientes { get; set; }
    public int CantidadAprobados { get; set; }
    public string? CausaImperfecto { get; set; }
    public string? Observaciones { get; set; }
    public string EstadoArreglos { get; set; } = string.Empty;
    public string? RevisadoPor { get; set; }
    public List<DetalleImperfectoDto> DetallesImperfectos { get; set; } = new();
}

public class DetalleImperfectoDto
{
    public string TipoDefecto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public string? Descripcion { get; set; }
}

public class CreateControlCalidadDto
{
    public int RemisionId { get; set; }
    public int CantidadImperfectos { get; set; }
    public int CantidadArreglos { get; set; }
    public int CantidadPendientes { get; set; }
    public int CantidadAprobados { get; set; }
    public string? CausaImperfecto { get; set; }
    public string? Observaciones { get; set; }
    public string? RevisadoPor { get; set; }
    public List<DetalleImperfectoDto> DetallesImperfectos { get; set; } = new();
}
