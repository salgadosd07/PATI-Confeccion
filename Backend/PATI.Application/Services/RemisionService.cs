using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IRemisionService
{
    Task<RemisionDto> CreateRemisionAsync(CreateRemisionDto dto);
    Task<RemisionDto?> GetRemisionByIdAsync(int id);
    Task<IEnumerable<RemisionDto>> GetAllRemisionesAsync();
    Task<IEnumerable<RemisionDto>> GetRemisionesByAsignacionAsync(int asignacionId);
    Task<RemisionDto> RegistrarRecepcionAsync(int id, RegistrarRecepcionDto dto);
}

public class RemisionService : IRemisionService
{
    private readonly IRepository<Remision> _remisionRepository;
    private readonly IRepository<AsignacionTaller> _asignacionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public RemisionService(
        IRepository<Remision> remisionRepository,
        IRepository<AsignacionTaller> asignacionRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _remisionRepository = remisionRepository;
        _asignacionRepository = asignacionRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<RemisionDto> CreateRemisionAsync(CreateRemisionDto dto)
    {
        var asignacion = await _asignacionRepository.GetByIdAsync(dto.AsignacionTallerId);
        if (asignacion == null)
            throw new Exception("Asignación no encontrada");

        var numeroRemision = GenerarNumeroRemision();

        var remision = new Remision
        {
            NumeroRemision = numeroRemision,
            AsignacionTallerId = dto.AsignacionTallerId,
            FechaDespacho = dto.FechaDespacho,
            CantidadEnviada = dto.CantidadEnviada,
            EstadoRemision = "Despachada",
            Observaciones = dto.Observaciones
        };

        await _remisionRepository.AddAsync(remision);
        await _unitOfWork.CompleteAsync();

        // Agregar detalles de la remisión
        if (dto.Detalles != null && dto.Detalles.Any())
        {
            foreach (var detalle in dto.Detalles)
            {
                _context.RemisionDetalles.Add(new RemisionDetalle
                {
                    RemisionId = remision.Id,
                    TallaId = detalle.TallaId,
                    ColorId = detalle.ColorId,
                    Cantidad = detalle.Cantidad
                });
            }
            await _unitOfWork.CompleteAsync();
        }

        return await GetRemisionByIdAsync(remision.Id) ?? throw new Exception("Error al crear la remisión");
    }

    public async Task<RemisionDto?> GetRemisionByIdAsync(int id)
    {
        var remision = await _context.Remisiones
            .Include(r => r.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .Include(r => r.AsignacionTaller)
                .ThenInclude(at => at.Referencia)
            .Include(r => r.Detalles)
                .ThenInclude(d => d.Talla)
            .Include(r => r.Detalles)
                .ThenInclude(d => d.Color)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (remision == null) return null;

        return MapToDto(remision);
    }

    public async Task<IEnumerable<RemisionDto>> GetAllRemisionesAsync()
    {
        var remisiones = await _context.Remisiones
            .Include(r => r.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .Include(r => r.AsignacionTaller)
                .ThenInclude(at => at.Referencia)
            .Include(r => r.Detalles)
                .ThenInclude(d => d.Talla)
            .Include(r => r.Detalles)
                .ThenInclude(d => d.Color)
            .OrderByDescending(r => r.FechaDespacho)
            .ToListAsync();

        return remisiones.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<RemisionDto>> GetRemisionesByAsignacionAsync(int asignacionId)
    {
        var remisiones = await _context.Remisiones
            .Where(r => r.AsignacionTallerId == asignacionId)
            .Include(r => r.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .Include(r => r.Detalles)
                .ThenInclude(d => d.Talla)
            .Include(r => r.Detalles)
                .ThenInclude(d => d.Color)
            .OrderByDescending(r => r.FechaDespacho)
            .ToListAsync();

        return remisiones.Select(MapToDto).ToList();
    }

    public async Task<RemisionDto> RegistrarRecepcionAsync(int id, RegistrarRecepcionDto dto)
    {
        var remision = await _remisionRepository.GetByIdAsync(id);
        if (remision == null)
            throw new Exception("Remisión no encontrada");

        remision.FechaRecepcion = dto.FechaRecepcion;
        remision.CantidadRecibida = dto.CantidadRecibida;
        remision.RevisadoPor = dto.RevisadoPor;
        remision.EstadoRemision = "Recibida";
        remision.Observaciones = dto.Observaciones;

        await _unitOfWork.CompleteAsync();

        return await GetRemisionByIdAsync(id) ?? throw new Exception("Error al actualizar la remisión");
    }

    private RemisionDto MapToDto(Remision remision)
    {
        return new RemisionDto
        {
            Id = remision.Id,
            NumeroRemision = remision.NumeroRemision,
            AsignacionTallerId = remision.AsignacionTallerId,
            TallerNombre = remision.AsignacionTaller?.Taller?.Nombre,
            ReferenciaNombre = remision.AsignacionTaller?.Referencia?.Nombre,
            FechaDespacho = remision.FechaDespacho,
            FechaRecepcion = remision.FechaRecepcion,
            CantidadEnviada = remision.CantidadEnviada,
            CantidadRecibida = remision.CantidadRecibida,
            RevisadoPor = remision.RevisadoPor,
            EstadoRemision = remision.EstadoRemision,
            Observaciones = remision.Observaciones,
            Detalles = remision.Detalles?.Select(d => new RemisionDetalleDto
            {
                TallaId = d.TallaId,
                TallaNombre = d.Talla?.Nombre,
                ColorId = d.ColorId,
                ColorNombre = d.Color?.Nombre,
                Cantidad = d.Cantidad
            }).ToList() ?? new List<RemisionDetalleDto>()
        };
    }

    private string GenerarNumeroRemision()
    {
        return $"REM-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
