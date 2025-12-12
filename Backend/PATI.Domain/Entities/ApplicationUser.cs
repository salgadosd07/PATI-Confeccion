using Microsoft.AspNetCore.Identity;

namespace PATI.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime? FechaCreacion { get; set; }
    public bool Activo { get; set; } = true;
}
