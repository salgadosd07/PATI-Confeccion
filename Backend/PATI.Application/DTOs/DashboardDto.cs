namespace PATI.Application.DTOs;

public class DashboardDto
{
    public ResumenProduccion ResumenProduccion { get; set; } = new();
    public List<AvancePorTaller> AvancesPorTaller { get; set; } = new();
    public List<AvancePorReferencia> AvancesPorReferencia { get; set; } = new();
    public List<AlertaProduccion> Alertas { get; set; } = new();
}

public class ResumenProduccion
{
    public int TotalCortes { get; set; }
    public int TotalPrendasProgramadas { get; set; }
    public int PrendasEnProceso { get; set; }
    public int PrendasTerminadas { get; set; }
    public int PrendasEnTransito { get; set; }
    public decimal PorcentajeAvanceGeneral { get; set; }
}

public class AvancePorTaller
{
    public int TallerId { get; set; }
    public string TallerNombre { get; set; } = string.Empty;
    public int CantidadAsignada { get; set; }
    public int CantidadLista { get; set; }
    public int CantidadEnProceso { get; set; }
    public decimal PorcentajeAvance { get; set; }
    public int AsignacionesActivas { get; set; }
}

public class AvancePorReferencia
{
    public int ReferenciaId { get; set; }
    public string ReferenciaNombre { get; set; } = string.Empty;
    public int CantidadProgramada { get; set; }
    public int CantidadTerminada { get; set; }
    public decimal PorcentajeAvance { get; set; }
}

public class AlertaProduccion
{
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public string? EntidadRelacionada { get; set; }
    public int? EntidadId { get; set; }
    public DateTime Fecha { get; set; }
}
