namespace PATI.Domain.Entities;

public class RemisionDetalle
{
    public int Id { get; set; }
    public int RemisionId { get; set; }
    public int TallaId { get; set; }
    public int ColorId { get; set; }
    public int Cantidad { get; set; }
    
    public Remision Remision { get; set; } = null!;
    public Talla Talla { get; set; } = null!;
    public Color Color { get; set; } = null!;
}
