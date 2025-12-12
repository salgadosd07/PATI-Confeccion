namespace PATI.Domain.Entities;

public class CorteColor
{
    public int CorteId { get; set; }
    public int ColorId { get; set; }
    public int Cantidad { get; set; }
    
    public Corte Corte { get; set; } = null!;
    public Color Color { get; set; } = null!;
}
