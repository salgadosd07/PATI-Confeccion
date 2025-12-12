using PATI.Application.DTOs;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IReportesService
{
    Task<ReportePorReferenciaDto> GetReportePorReferenciaAsync(int referenciaId);
    Task<ReportePorTallerDto> GetReportePorTallerAsync(int tallerId);
    Task<ReporteFinancieroDto> GetReporteFinancieroAsync(DateTime? fechaInicio, DateTime? fechaFin);
    Task<IEnumerable<ReporteColorDto>> GetReporteColoresAsync();
}

public class ReportesService : IReportesService
{
    private readonly ApplicationDbContext _context;

    public ReportesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReportePorReferenciaDto> GetReportePorReferenciaAsync(int referenciaId)
    {
        var referencia = await _context.Referencias.FindAsync(referenciaId);
        if (referencia == null)
            throw new Exception("Referencia no encontrada");

        var cortes = await _context.Cortes
            .Where(c => c.ReferenciaId == referenciaId)
            .ToListAsync();

        var asignaciones = await _context.AsignacionesTaller
            .Where(a => a.ReferenciaId == referenciaId)
            .Include(a => a.Avances)
            .ToListAsync();

        var controlesCalidad = await _context.ControlesCalidad
            .Where(c => c.Remision.AsignacionTaller.ReferenciaId == referenciaId)
            .ToListAsync();

        return new ReportePorReferenciaDto
        {
            ReferenciaId = referenciaId,
            ReferenciaNombre = referencia.Nombre,
            CantidadTotalProgramada = cortes.Sum(c => c.CantidadProgramada),
            CantidadTotalCortada = cortes.Sum(c => c.CantidadTotal),
            CantidadAsignada = asignaciones.Sum(a => a.CantidadAsignada),
            CantidadTerminada = asignaciones.SelectMany(a => a.Avances).Sum(av => av.CantidadLista),
            TotalImperfectos = controlesCalidad.Sum(c => c.CantidadImperfectos),
            TotalArreglos = controlesCalidad.Sum(c => c.CantidadArreglos),
            TotalPendientes = controlesCalidad.Sum(c => c.CantidadPendientes),
            TotalAprobados = controlesCalidad.Sum(c => c.CantidadAprobados)
        };
    }

    public async Task<ReportePorTallerDto> GetReportePorTallerAsync(int tallerId)
    {
        var taller = await _context.Talleres.FindAsync(tallerId);
        if (taller == null)
            throw new Exception("Taller no encontrado");

        var asignaciones = await _context.AsignacionesTaller
            .Where(a => a.TallerId == tallerId)
            .Include(a => a.Avances)
            .Include(a => a.Remisiones)
            .ToListAsync();

        var controlesCalidad = await _context.ControlesCalidad
            .Where(c => c.Remision.AsignacionTaller.TallerId == tallerId)
            .ToListAsync();

        var asignacionesCompletadas = asignaciones
            .Where(a => a.FechaEstimadaEntrega.HasValue && a.Avances.Any(av => av.CantidadLista == a.CantidadAsignada))
            .ToList();

        var tiemposEntrega = asignacionesCompletadas
            .Select(a => a.FechaEstimadaEntrega.HasValue 
                ? (a.Remisiones.FirstOrDefault()?.FechaDespacho ?? DateTime.Now) - a.FechaAsignacion 
                : TimeSpan.Zero)
            .Where(t => t != TimeSpan.Zero)
            .ToList();

        return new ReportePorTallerDto
        {
            TallerId = tallerId,
            TallerNombre = taller.Nombre,
            TotalAsignaciones = asignaciones.Count,
            ProduccionTotal = asignaciones.Sum(a => a.CantidadAsignada),
            ProduccionTerminada = asignaciones.SelectMany(a => a.Avances).Sum(av => av.CantidadLista),
            RendimientoPromedio = asignaciones.Count > 0 
                ? (decimal)asignaciones.SelectMany(a => a.Avances).Sum(av => av.CantidadLista) / asignaciones.Sum(a => a.CantidadAsignada) * 100 
                : 0,
            TiempoPromedioEntrega = tiemposEntrega.Any() ? tiemposEntrega.Average(t => t.TotalDays) : 0,
            ImperfectosRecurrentes = controlesCalidad.Sum(c => c.CantidadImperfectos),
            TotalPagado = await _context.Pagos
                .Where(p => p.AsignacionTaller.TallerId == tallerId && p.EstadoPago == "Pagado")
                .SumAsync(p => p.MontoPagado ?? 0)
        };
    }

    public async Task<ReporteFinancieroDto> GetReporteFinancieroAsync(DateTime? fechaInicio, DateTime? fechaFin)
    {
        fechaInicio ??= DateTime.Now.AddMonths(-1);
        fechaFin ??= DateTime.Now;

        var pagos = await _context.Pagos
            .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin)
            .Include(p => p.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .ToListAsync();

        var asignaciones = await _context.AsignacionesTaller
            .Where(a => a.FechaAsignacion >= fechaInicio && a.FechaAsignacion <= fechaFin)
            .ToListAsync();

        return new ReporteFinancieroDto
        {
            FechaInicio = fechaInicio.Value,
            FechaFin = fechaFin.Value,
            TotalPagosProgramados = pagos.Sum(p => p.MontoTotal),
            TotalPagosPagados = pagos.Where(p => p.EstadoPago == "Pagado").Sum(p => p.MontoPagado ?? 0),
            TotalPagosPendientes = pagos.Where(p => p.EstadoPago == "Pendiente").Sum(p => p.MontoTotal),
            TotalPagosParciales = pagos.Where(p => p.EstadoPago == "Parcial").Sum(p => p.MontoTotal - (p.MontoPagado ?? 0)),
            CostoPromedioUnidad = asignaciones.Any() && asignaciones.Sum(a => a.CantidadAsignada) > 0
                ? pagos.Sum(p => p.MontoTotal) / asignaciones.Sum(a => a.CantidadAsignada)
                : 0,
            PagosPorTaller = pagos
                .GroupBy(p => new { p.AsignacionTaller.TallerId, p.AsignacionTaller.Taller.Nombre })
                .Select(g => new PagoTallerDto
                {
                    TallerId = g.Key.TallerId,
                    TallerNombre = g.Key.Nombre ?? "Sin nombre",
                    TotalPagado = g.Where(p => p.EstadoPago == "Pagado").Sum(p => p.MontoPagado ?? 0),
                    TotalPendiente = g.Where(p => p.EstadoPago != "Pagado").Sum(p => p.MontoTotal - (p.MontoPagado ?? 0))
                })
                .ToList()
        };
    }

    public async Task<IEnumerable<ReporteColorDto>> GetReporteColoresAsync()
    {
        try
        {
            // Obtener todos los colores con sus cantidades
            var coloresConCantidades = await _context.CorteColores
                .Include(cc => cc.Color)
                .Include(cc => cc.Corte)
                    .ThenInclude(c => c.Referencia)
                .Where(cc => cc.Color != null && cc.Corte != null)
                .ToListAsync();

            // Agrupar por color
            var corteColoresAgrupados = coloresConCantidades
                .GroupBy(cc => new { cc.ColorId, ColorNombre = cc.Color.Nombre })
                .Select(g => new
                {
                    ColorId = g.Key.ColorId,
                    ColorNombre = g.Key.ColorNombre,
                    CantidadTotal = g.Sum(cc => cc.Cantidad),
                    Referencias = g.Where(cc => cc.Corte.Referencia != null)
                                   .Select(cc => cc.Corte.Referencia.Nombre)
                                   .Distinct()
                                   .ToList()
                })
                .ToList();

            // Obtener inventario por color
            var inventarioColores = await _context.Inventarios
                .GroupBy(i => i.ColorId)
                .Select(g => new
                {
                    ColorId = g.Key,
                    CantidadDisponible = g.Sum(i => i.CantidadDisponible)
                })
                .ToListAsync();

            // Combinar resultados
            var resultado = corteColoresAgrupados.Select(cc => new ReporteColorDto
            {
                ColorId = cc.ColorId,
                ColorNombre = cc.ColorNombre ?? "Sin nombre",
                CantidadTotal = cc.CantidadTotal,
                CantidadDisponible = inventarioColores.FirstOrDefault(ic => ic.ColorId == cc.ColorId)?.CantidadDisponible ?? 0,
                ReferenciasUsadas = cc.Referencias
            })
            .OrderByDescending(r => r.CantidadTotal)
            .ToList();

            return resultado;
        }
        catch (Exception ex)
        {
            // Log the error and return empty list
            Console.WriteLine($"Error en GetReporteColoresAsync: {ex.Message}");
            return new List<ReporteColorDto>();
        }
    }
}
