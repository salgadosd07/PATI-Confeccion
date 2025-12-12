namespace PATI.Domain.Entities;

public class Material : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? UnidadMedida { get; set; }
    
    public ICollection<Corte> Cortes { get; set; } = new List<Corte>();
}
