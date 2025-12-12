using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync();
}

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        var totalCortes = await _context.Cortes.CountAsync();
        var totalPrendasProgramadas = await _context.Cortes.SumAsync(c => c.CantidadProgramada);
        
        var asignaciones = await _context.AsignacionesTaller
            .Include(a => a.Avances)
            .Include(a => a.Taller)
            .Include(a => a.Referencia)
            .ToListAsync();

        var prendasEnProceso = asignaciones.Sum(a => 
            a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadEnProceso ?? 0);
        
        var prendasTerminadas = asignaciones.Sum(a => 
            a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadLista ?? 0);
        
        var prendasEnTransito = asignaciones.Sum(a => 
            a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadDespachada ?? 0);

        var porcentajeGeneral = totalPrendasProgramadas > 0 
            ? (decimal)prendasTerminadas / totalPrendasProgramadas * 100 
            : 0;

        var avancesPorTaller = asignaciones
            .GroupBy(a => new { a.TallerId, a.Taller!.Nombre })
            .Select(g => new AvancePorTaller
            {
                TallerId = g.Key.TallerId,
                TallerNombre = g.Key.Nombre,
                CantidadAsignada = g.Sum(a => a.CantidadAsignada),
                CantidadLista = g.Sum(a => a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadLista ?? 0),
                CantidadEnProceso = g.Sum(a => a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadEnProceso ?? 0),
                PorcentajeAvance = g.Sum(a => a.CantidadAsignada) > 0 
                    ? (decimal)g.Sum(a => a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadLista ?? 0) / g.Sum(a => a.CantidadAsignada) * 100 
                    : 0,
                AsignacionesActivas = g.Count()
            })
            .ToList();

        var avancesPorReferencia = asignaciones
            .GroupBy(a => new { a.ReferenciaId, a.Referencia!.Nombre })
            .Select(g => new AvancePorReferencia
            {
                ReferenciaId = g.Key.ReferenciaId,
                ReferenciaNombre = g.Key.Nombre,
                CantidadProgramada = g.Sum(a => a.CantidadAsignada),
                CantidadTerminada = g.Sum(a => a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadLista ?? 0),
                PorcentajeAvance = g.Sum(a => a.CantidadAsignada) > 0 
                    ? (decimal)g.Sum(a => a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.CantidadLista ?? 0) / g.Sum(a => a.CantidadAsignada) * 100 
                    : 0
            })
            .ToList();

        var alertas = new List<AlertaProduccion>();
        
        var asignacionesAtrasadas = asignaciones.Where(a => 
            a.FechaEstimadaEntrega.HasValue && 
            a.FechaEstimadaEntrega.Value < DateTime.Now &&
            (a.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault()?.PorcentajeAvance ?? 0) < 100);

        foreach (var asignacion in asignacionesAtrasadas)
        {
            alertas.Add(new AlertaProduccion
            {
                Tipo = "Retraso",
                Mensaje = $"Asignación {asignacion.CodigoAsignacion} del taller {asignacion.Taller?.Nombre} está atrasada",
                EntidadRelacionada = "AsignacionTaller",
                EntidadId = asignacion.Id,
                Fecha = DateTime.Now
            });
        }

        return new DashboardDto
        {
            ResumenProduccion = new ResumenProduccion
            {
                TotalCortes = totalCortes,
                TotalPrendasProgramadas = totalPrendasProgramadas,
                PrendasEnProceso = prendasEnProceso,
                PrendasTerminadas = prendasTerminadas,
                PrendasEnTransito = prendasEnTransito,
                PorcentajeAvanceGeneral = porcentajeGeneral
            },
            AvancesPorTaller = avancesPorTaller,
            AvancesPorReferencia = avancesPorReferencia,
            Alertas = alertas
        };
    }
}
