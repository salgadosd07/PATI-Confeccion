using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PATI.Application.Services;
using PATI.Domain.Entities;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacionesController : ControllerBase
{
    private readonly INotificacionService _notificacionService;

    public NotificacionesController(INotificacionService notificacionService)
    {
        _notificacionService = notificacionService;
    }

    [HttpGet("no-leidas")]
    public async Task<ActionResult<IEnumerable<Notificacion>>> GetNoLeidas()
    {
        var usuarioId = User.FindFirst("sub")?.Value ?? User.Identity?.Name;
        if (string.IsNullOrEmpty(usuarioId))
            return Unauthorized();

        var notificaciones = await _notificacionService.GetNotificacionesNoLeidasAsync(usuarioId);
        return Ok(notificaciones);
    }

    [HttpPut("{id}/marcar-leida")]
    public async Task<ActionResult> MarcarLeida(int id)
    {
        await _notificacionService.MarcarComoLeidaAsync(id);
        return Ok(new { message = "Notificación marcada como leída" });
    }

    [HttpPost("verificar-retrasos")]
    [Authorize(Roles = "Administrador,Corte")]
    public async Task<ActionResult> VerificarRetrasos()
    {
        // Este endpoint se puede llamar manualmente o programarse con un job
        return Ok(new { message = "Verificación de retrasos ejecutada" });
    }
}
