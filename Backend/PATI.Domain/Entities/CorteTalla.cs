namespace PATI.Domain.Entities;

public class CorteTalla
{
    public int CorteId { get; set; }
    public int TallaId { get; set; }
    public int Cantidad { get; set; }
    
    public Corte Corte { get; set; } = null!;
    public Talla Talla { get; set; } = null!;
}
