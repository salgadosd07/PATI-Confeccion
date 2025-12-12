using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IAsignacionTallerService
{
    Task<AsignacionTallerDto> CreateAsignacionAsync(CreateAsignacionTallerDto dto);
    Task<AsignacionTallerDto?> GetAsignacionByIdAsync(int id);
    Task<IEnumerable<AsignacionTallerDto>> GetAsignacionesByTallerAsync(int tallerId);
    Task<IEnumerable<AsignacionTallerDto>> GetAllAsignacionesAsync();
}

public class AsignacionTallerService : IAsignacionTallerService
{
    private readonly IRepository<AsignacionTaller> _asignacionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public AsignacionTallerService(
        IRepository<AsignacionTaller> asignacionRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _asignacionRepository = asignacionRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<AsignacionTallerDto> CreateAsignacionAsync(CreateAsignacionTallerDto dto)
    {
        var codigoAsignacion = GenerarCodigoAsignacion();
        var valorTotal = dto.ValorUnitario.HasValue ? dto.ValorUnitario.Value * dto.CantidadAsignada : 0;

        var asignacion = new AsignacionTaller
        {
            CodigoAsignacion = codigoAsignacion,
            TallerId = dto.TallerId,
            ReferenciaId = dto.ReferenciaId,
            CorteId = dto.CorteId,
            FechaAsignacion = dto.FechaAsignacion,
            FechaEstimadaEntrega = dto.FechaEstimadaEntrega,
            CantidadAsignada = dto.CantidadAsignada,
            ValorUnitario = dto.ValorUnitario,
            ValorTotal = valorTotal,
            Observaciones = dto.Observaciones
        };

        await _asignacionRepository.AddAsync(asignacion);
        await _unitOfWork.CompleteAsync();

        return await GetAsignacionByIdAsync(asignacion.Id) ?? throw new Exception("Error al crear la asignación");
    }

    public async Task<AsignacionTallerDto?> GetAsignacionByIdAsync(int id)
    {
        var asignacion = await _context.AsignacionesTaller
            .Include(a => a.Taller)
            .Include(a => a.Referencia)
            .Include(a => a.Corte)
            .Include(a => a.Avances)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (asignacion == null) return null;

        var ultimoAvance = asignacion.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault();

        return new AsignacionTallerDto
        {
            Id = asignacion.Id,
            CodigoAsignacion = asignacion.CodigoAsignacion,
            TallerId = asignacion.TallerId,
            TallerNombre = asignacion.Taller?.Nombre,
            ReferenciaId = asignacion.ReferenciaId,
            ReferenciaNombre = asignacion.Referencia?.Nombre,
            CorteId = asignacion.CorteId,
            CodigoLote = asignacion.Corte?.CodigoLote,
            FechaAsignacion = asignacion.FechaAsignacion,
            FechaEstimadaEntrega = asignacion.FechaEstimadaEntrega,
            CantidadAsignada = asignacion.CantidadAsignada,
            ValorUnitario = asignacion.ValorUnitario,
            ValorTotal = asignacion.ValorTotal,
            Observaciones = asignacion.Observaciones,
            PorcentajeAvance = ultimoAvance?.PorcentajeAvance
        };
    }

    public async Task<IEnumerable<AsignacionTallerDto>> GetAsignacionesByTallerAsync(int tallerId)
    {
        var asignaciones = await _context.AsignacionesTaller
            .Include(a => a.Taller)
            .Include(a => a.Referencia)
            .Include(a => a.Corte)
            .Include(a => a.Avances)
            .Where(a => a.TallerId == tallerId)
            .ToListAsync();

        return asignaciones.Select(asignacion =>
        {
            var ultimoAvance = asignacion.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault();
            return new AsignacionTallerDto
            {
                Id = asignacion.Id,
                CodigoAsignacion = asignacion.CodigoAsignacion,
                TallerId = asignacion.TallerId,
                TallerNombre = asignacion.Taller?.Nombre,
                ReferenciaId = asignacion.ReferenciaId,
                ReferenciaNombre = asignacion.Referencia?.Nombre,
                CorteId = asignacion.CorteId,
                CodigoLote = asignacion.Corte?.CodigoLote,
                FechaAsignacion = asignacion.FechaAsignacion,
                FechaEstimadaEntrega = asignacion.FechaEstimadaEntrega,
                CantidadAsignada = asignacion.CantidadAsignada,
                ValorUnitario = asignacion.ValorUnitario,
                ValorTotal = asignacion.ValorTotal,
                Observaciones = asignacion.Observaciones,
                PorcentajeAvance = ultimoAvance?.PorcentajeAvance
            };
        });
    }

    public async Task<IEnumerable<AsignacionTallerDto>> GetAllAsignacionesAsync()
    {
        var asignaciones = await _context.AsignacionesTaller
            .Include(a => a.Taller)
            .Include(a => a.Referencia)
            .Include(a => a.Corte)
            .Include(a => a.Avances)
            .ToListAsync();

        return asignaciones.Select(asignacion =>
        {
            var ultimoAvance = asignacion.Avances.OrderByDescending(av => av.FechaReporte).FirstOrDefault();
            return new AsignacionTallerDto
            {
                Id = asignacion.Id,
                CodigoAsignacion = asignacion.CodigoAsignacion,
                TallerId = asignacion.TallerId,
                TallerNombre = asignacion.Taller?.Nombre,
                ReferenciaId = asignacion.ReferenciaId,
                ReferenciaNombre = asignacion.Referencia?.Nombre,
                CorteId = asignacion.CorteId,
                CodigoLote = asignacion.Corte?.CodigoLote,
                FechaAsignacion = asignacion.FechaAsignacion,
                FechaEstimadaEntrega = asignacion.FechaEstimadaEntrega,
                CantidadAsignada = asignacion.CantidadAsignada,
                ValorUnitario = asignacion.ValorUnitario,
                ValorTotal = asignacion.ValorTotal,
                Observaciones = asignacion.Observaciones,
                PorcentajeAvance = ultimoAvance?.PorcentajeAvance
            };
        });
    }

    private string GenerarCodigoAsignacion()
    {
        return $"ASIG-{DateTime.Now:yyyyMMddHHmmss}";
    }
}
