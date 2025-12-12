namespace PATI.Domain.Entities;

public class Referencia : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string TipoPrenda { get; set; } = string.Empty;
    
    public ICollection<Corte> Cortes { get; set; } = new List<Corte>();
    public ICollection<AsignacionTaller> Asignaciones { get; set; } = new List<AsignacionTaller>();
}
