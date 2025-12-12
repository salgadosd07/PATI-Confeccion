namespace PATI.Domain.Entities;

public class Corte : BaseEntity
{
    public string CodigoLote { get; set; } = string.Empty;
    public string Mesa { get; set; } = string.Empty;
    public DateTime FechaCorte { get; set; }
    public int ReferenciaId { get; set; }
    public int MaterialId { get; set; }
    public int CantidadTotal { get; set; }
    public int CantidadProgramada { get; set; }
    
    public Referencia Referencia { get; set; } = null!;
    public Material Material { get; set; } = null!;
    public ICollection<CorteColor> CorteColores { get; set; } = new List<CorteColor>();
    public ICollection<CorteTalla> CorteTallas { get; set; } = new List<CorteTalla>();
}
