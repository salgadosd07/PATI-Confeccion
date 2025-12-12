using Microsoft.AspNetCore.Identity;
using PATI.Domain.Entities;

namespace PATI.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Crear roles
        var roles = new[] { "Administrador", "Corte", "Taller", "Calidad", "Bodega", "Financiero" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Crear usuario administrador por defecto
        var adminEmail = "admin@pati.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                NombreCompleto = "Administrador del Sistema",
                EmailConfirmed = true,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }
        }
    }
}
