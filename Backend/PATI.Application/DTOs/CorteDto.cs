using PATI.Domain.Entities;

namespace PATI.Application.DTOs;

public class CorteDto
{
    public int Id { get; set; }
    public string CodigoLote { get; set; } = string.Empty;
    public string Mesa { get; set; } = string.Empty;
    public DateTime FechaCorte { get; set; }
    public int ReferenciaId { get; set; }
    public string? ReferenciaNombre { get; set; }
    public int MaterialId { get; set; }
    public string? MaterialNombre { get; set; }
    public int CantidadTotal { get; set; }
    public int CantidadProgramada { get; set; }
    public List<CorteColorDto> Colores { get; set; } = new();
    public List<CorteTallaDto> Tallas { get; set; } = new();
}

public class CorteColorDto
{
    public int ColorId { get; set; }
    public string? ColorNombre { get; set; }
    public int Cantidad { get; set; }
}

public class CorteTallaDto
{
    public int TallaId { get; set; }
    public string? TallaNombre { get; set; }
    public int Cantidad { get; set; }
}

public class CreateCorteDto
{
    public string Mesa { get; set; } = string.Empty;
    public DateTime FechaCorte { get; set; }
    public int ReferenciaId { get; set; }
    public int MaterialId { get; set; }
    public int CantidadProgramada { get; set; }
    public List<CorteColorDto> Colores { get; set; } = new();
    public List<CorteTallaDto> Tallas { get; set; } = new();
}
