namespace PATI.Domain.Entities;

public class Taller : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? NombreContacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public string? Observaciones { get; set; }
    
    public ICollection<AsignacionTaller> Asignaciones { get; set; } = new List<AsignacionTaller>();
}
