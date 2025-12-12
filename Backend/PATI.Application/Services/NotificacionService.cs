using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface INotificacionService
{
    Task EnviarNotificacionRetrasoAsync(int asignacionId);
    Task EnviarNotificacionImperfectosAsync(int controlCalidadId);
    Task EnviarNotificacionPagoAsync(int pagoId, string estado);
    Task<IEnumerable<Notificacion>> GetNotificacionesNoLeidasAsync(string usuarioId);
    Task MarcarComoLeidaAsync(int notificacionId);
}

public class NotificacionService : INotificacionService
{
    private readonly IRepository<Notificacion> _notificacionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public NotificacionService(
        IRepository<Notificacion> notificacionRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _notificacionRepository = notificacionRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task EnviarNotificacionRetrasoAsync(int asignacionId)
    {
        var asignacion = await _context.AsignacionesTaller
            .Include(a => a.Taller)
            .Include(a => a.Referencia)
            .FirstOrDefaultAsync(a => a.Id == asignacionId);

        if (asignacion == null || !asignacion.FechaEstimadaEntrega.HasValue)
            return;

        if (DateTime.Now > asignacion.FechaEstimadaEntrega.Value)
        {
            var diasRetraso = (int)(DateTime.Now - asignacion.FechaEstimadaEntrega.Value).TotalDays;

            var notificacion = new Notificacion
            {
                Tipo = "Retraso",
                Titulo = "Asignación Retrasada",
                Mensaje = $"La asignación {asignacion.CodigoAsignacion} del taller {asignacion.Taller.Nombre} " +
                         $"está retrasada {diasRetraso} día(s). Referencia: {asignacion.Referencia.Nombre}",
                DestinatarioEmail = asignacion.Taller.Email,
                DestinatarioTelefono = asignacion.Taller.Telefono,
                FechaEnvio = DateTime.Now,
                Enviada = false,
                Leida = false,
                EntidadRelacionada = "AsignacionTaller",
                EntidadRelacionadaId = asignacionId
            };

            await _notificacionRepository.AddAsync(notificacion);
            await _unitOfWork.CompleteAsync();

            // Aquí se podría implementar envío real por email/WhatsApp
            await SimularEnvioNotificacionAsync(notificacion);
        }
    }

    public async Task EnviarNotificacionImperfectosAsync(int controlCalidadId)
    {
        var control = await _context.ControlesCalidad
            .Include(c => c.Remision)
                .ThenInclude(r => r.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .FirstOrDefaultAsync(c => c.Id == controlCalidadId);

        if (control == null || control.CantidadImperfectos == 0)
            return;

        var notificacion = new Notificacion
        {
            Tipo = "Calidad",
            Titulo = "Imperfectos Detectados",
            Mensaje = $"Se detectaron {control.CantidadImperfectos} imperfectos en la remisión {control.Remision.NumeroRemision}. " +
                     $"Arreglos requeridos: {control.CantidadArreglos}. " +
                     $"Causa: {control.CausaImperfecto ?? "No especificada"}",
            DestinatarioEmail = control.Remision.AsignacionTaller.Taller.Email,
            DestinatarioTelefono = control.Remision.AsignacionTaller.Taller.Telefono,
            FechaEnvio = DateTime.Now,
            Enviada = false,
            Leida = false,
            EntidadRelacionada = "ControlCalidad",
            EntidadRelacionadaId = controlCalidadId
        };

        await _notificacionRepository.AddAsync(notificacion);
        await _unitOfWork.CompleteAsync();

        await SimularEnvioNotificacionAsync(notificacion);
    }

    public async Task EnviarNotificacionPagoAsync(int pagoId, string estado)
    {
        var pago = await _context.Pagos
            .Include(p => p.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .FirstOrDefaultAsync(p => p.Id == pagoId);

        if (pago == null)
            return;

        var mensaje = estado switch
        {
            "Aprobado" => $"El pago {pago.NumeroPago} por ${pago.MontoTotal:N2} ha sido aprobado.",
            "Rechazado" => $"El pago {pago.NumeroPago} por ${pago.MontoTotal:N2} ha sido rechazado.",
            "Pagado" => $"El pago {pago.NumeroPago} por ${pago.MontoPagado:N2} ha sido procesado exitosamente.",
            _ => $"Estado del pago {pago.NumeroPago} actualizado: {estado}"
        };

        var notificacion = new Notificacion
        {
            Tipo = "Pago",
            Titulo = $"Notificación de Pago - {estado}",
            Mensaje = mensaje,
            DestinatarioEmail = pago.AsignacionTaller.Taller.Email,
            DestinatarioTelefono = pago.AsignacionTaller.Taller.Telefono,
            FechaEnvio = DateTime.Now,
            Enviada = false,
            Leida = false,
            EntidadRelacionada = "Pago",
            EntidadRelacionadaId = pagoId
        };

        await _notificacionRepository.AddAsync(notificacion);
        await _unitOfWork.CompleteAsync();

        await SimularEnvioNotificacionAsync(notificacion);
    }

    public async Task<IEnumerable<Notificacion>> GetNotificacionesNoLeidasAsync(string usuarioId)
    {
        return await _context.Notificaciones
            .Where(n => n.DestinatarioId == usuarioId && !n.Leida)
            .OrderByDescending(n => n.FechaEnvio)
            .ToListAsync();
    }

    public async Task MarcarComoLeidaAsync(int notificacionId)
    {
        var notificacion = await _notificacionRepository.GetByIdAsync(notificacionId);
        if (notificacion != null)
        {
            notificacion.Leida = true;
            await _unitOfWork.CompleteAsync();
        }
    }

    private async Task SimularEnvioNotificacionAsync(Notificacion notificacion)
    {
        // Simulación de envío - aquí se integraría con servicios reales
        // como SendGrid, Twilio, etc.
        await Task.Delay(100);
        
        notificacion.Enviada = true;
        await _unitOfWork.CompleteAsync();

        // TODO: Implementar envío real por email
        // TODO: Implementar envío real por WhatsApp
    }
}
