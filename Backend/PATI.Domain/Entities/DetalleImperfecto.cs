namespace PATI.Domain.Entities;

public class DetalleImperfecto
{
    public int Id { get; set; }
    public int ControlCalidadId { get; set; }
    public string TipoDefecto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public string? Descripcion { get; set; }
    
    public ControlCalidad ControlCalidad { get; set; } = null!;
}
