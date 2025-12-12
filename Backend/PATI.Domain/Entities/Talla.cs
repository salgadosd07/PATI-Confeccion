namespace PATI.Domain.Entities;

public class Talla : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    
    public ICollection<CorteTalla> CorteTallas { get; set; } = new List<CorteTalla>();
}
