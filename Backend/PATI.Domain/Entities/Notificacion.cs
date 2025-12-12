namespace PATI.Domain.Entities;

public class Notificacion : BaseEntity
{
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public string? DestinatarioId { get; set; }
    public string? DestinatarioEmail { get; set; }
    public string? DestinatarioTelefono { get; set; }
    public DateTime FechaEnvio { get; set; }
    public bool Enviada { get; set; }
    public bool Leida { get; set; }
    public string? EntidadRelacionada { get; set; }
    public int? EntidadRelacionadaId { get; set; }
    
}
