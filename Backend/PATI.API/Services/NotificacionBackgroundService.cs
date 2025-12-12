using PATI.Application.Services;
using PATI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PATI.API.Services;

public class NotificacionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificacionBackgroundService> _logger;

    public NotificacionBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<NotificacionBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de notificaciones iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await VerificarRetrasosAsync();
                
                // Ejecutar cada 6 horas
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio de notificaciones");
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }

    private async Task VerificarRetrasosAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var notificacionService = scope.ServiceProvider.GetRequiredService<INotificacionService>();

        try
        {
            var asignacionesRetrasadas = await context.AsignacionesTaller
                .Where(a => a.FechaEstimadaEntrega.HasValue && 
                           a.FechaEstimadaEntrega.Value < DateTime.Now)
                .ToListAsync();

            foreach (var asignacion in asignacionesRetrasadas)
            {
                try
                {
                    await notificacionService.EnviarNotificacionRetrasoAsync(asignacion.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error enviando notificación para asignación {asignacion.Id}");
                }
            }

            _logger.LogInformation($"Verificación de retrasos completada. {asignacionesRetrasadas.Count} asignaciones retrasadas encontradas");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando retrasos");
        }
    }
}
