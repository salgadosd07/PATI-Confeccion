namespace PATI.Domain.Entities;

public class Color : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoHex { get; set; }
    
    public ICollection<CorteColor> CorteColores { get; set; } = new List<CorteColor>();
}
