using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IAvanceTallerService
{
    Task<AvanceTallerDto> CreateAvanceAsync(CreateAvanceTallerDto dto);
    Task<AvanceTallerDto?> GetAvanceByIdAsync(int id);
    Task<IEnumerable<AvanceTallerDto>> GetAvancesByAsignacionAsync(int asignacionId);
    Task<AvanceTallerDto?> GetUltimoAvanceAsync(int asignacionId);
}

public class AvanceTallerService : IAvanceTallerService
{
    private readonly IRepository<AvanceTaller> _avanceRepository;
    private readonly IRepository<AsignacionTaller> _asignacionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public AvanceTallerService(
        IRepository<AvanceTaller> avanceRepository,
        IRepository<AsignacionTaller> asignacionRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _avanceRepository = avanceRepository;
        _asignacionRepository = asignacionRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<AvanceTallerDto> CreateAvanceAsync(CreateAvanceTallerDto dto)
    {
        var asignacion = await _asignacionRepository.GetByIdAsync(dto.AsignacionTallerId);
        if (asignacion == null)
            throw new Exception("Asignación no encontrada");

        var porcentajeAvance = asignacion.CantidadAsignada > 0 
            ? (decimal)dto.CantidadLista / asignacion.CantidadAsignada * 100 
            : 0;

        var avance = new AvanceTaller
        {
            AsignacionTallerId = dto.AsignacionTallerId,
            FechaReporte = DateTime.Now,
            CantidadLista = dto.CantidadLista,
            CantidadEnProceso = dto.CantidadEnProceso,
            CantidadPendiente = dto.CantidadPendiente,
            CantidadDespachada = dto.CantidadDespachada,
            PorcentajeAvance = porcentajeAvance,
            Observaciones = dto.Observaciones,
            UrlFotoEvidencia = dto.UrlFotoEvidencia
        };

        await _avanceRepository.AddAsync(avance);
        await _unitOfWork.CompleteAsync();

        return await GetAvanceByIdAsync(avance.Id) ?? throw new Exception("Error al crear el avance");
    }

    public async Task<AvanceTallerDto?> GetAvanceByIdAsync(int id)
    {
        var avance = await _context.AvancesTaller
            .Include(a => a.AsignacionTaller)
            .ThenInclude(at => at.Taller)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (avance == null) return null;

        return new AvanceTallerDto
        {
            Id = avance.Id,
            AsignacionTallerId = avance.AsignacionTallerId,
            FechaReporte = avance.FechaReporte,
            CantidadLista = avance.CantidadLista,
            CantidadEnProceso = avance.CantidadEnProceso,
            CantidadPendiente = avance.CantidadPendiente,
            CantidadDespachada = avance.CantidadDespachada,
            PorcentajeAvance = avance.PorcentajeAvance,
            Observaciones = avance.Observaciones,
            UrlFotoEvidencia = avance.UrlFotoEvidencia
        };
    }

    public async Task<IEnumerable<AvanceTallerDto>> GetAvancesByAsignacionAsync(int asignacionId)
    {
        var avances = await _context.AvancesTaller
            .Where(a => a.AsignacionTallerId == asignacionId)
            .OrderByDescending(a => a.FechaReporte)
            .ToListAsync();

        return avances.Select(avance => new AvanceTallerDto
        {
            Id = avance.Id,
            AsignacionTallerId = avance.AsignacionTallerId,
            FechaReporte = avance.FechaReporte,
            CantidadLista = avance.CantidadLista,
            CantidadEnProceso = avance.CantidadEnProceso,
            CantidadPendiente = avance.CantidadPendiente,
            CantidadDespachada = avance.CantidadDespachada,
            PorcentajeAvance = avance.PorcentajeAvance,
            Observaciones = avance.Observaciones,
            UrlFotoEvidencia = avance.UrlFotoEvidencia
        }).ToList();
    }

    public async Task<AvanceTallerDto?> GetUltimoAvanceAsync(int asignacionId)
    {
        var avance = await _context.AvancesTaller
            .Where(a => a.AsignacionTallerId == asignacionId)
            .OrderByDescending(a => a.FechaReporte)
            .FirstOrDefaultAsync();

        if (avance == null) return null;

        return new AvanceTallerDto
        {
            Id = avance.Id,
            AsignacionTallerId = avance.AsignacionTallerId,
            FechaReporte = avance.FechaReporte,
            CantidadLista = avance.CantidadLista,
            CantidadEnProceso = avance.CantidadEnProceso,
            CantidadPendiente = avance.CantidadPendiente,
            CantidadDespachada = avance.CantidadDespachada,
            PorcentajeAvance = avance.PorcentajeAvance,
            Observaciones = avance.Observaciones,
            UrlFotoEvidencia = avance.UrlFotoEvidencia
        };
    }
}
