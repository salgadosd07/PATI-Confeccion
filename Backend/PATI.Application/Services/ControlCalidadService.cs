using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IControlCalidadService
{
    Task<ControlCalidadDto> CreateControlAsync(CreateControlCalidadDto dto);
    Task<ControlCalidadDto?> GetControlByIdAsync(int id);
    Task<IEnumerable<ControlCalidadDto>> GetControlesByRemisionAsync(int remisionId);
    Task<ControlCalidadDto> ActualizarEstadoArreglosAsync(int id, string estado);
}

public class ControlCalidadService : IControlCalidadService
{
    private readonly IRepository<ControlCalidad> _controlCalidadRepository;
    private readonly IRepository<Remision> _remisionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly INotificacionService _notificacionService;

    public ControlCalidadService(
        IRepository<ControlCalidad> controlCalidadRepository,
        IRepository<Remision> remisionRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context,
        INotificacionService notificacionService)
    {
        _controlCalidadRepository = controlCalidadRepository;
        _remisionRepository = remisionRepository;
        _unitOfWork = unitOfWork;
        _context = context;
        _notificacionService = notificacionService;
    }

    public async Task<ControlCalidadDto> CreateControlAsync(CreateControlCalidadDto dto)
    {
        var remision = await _remisionRepository.GetByIdAsync(dto.RemisionId);
        if (remision == null)
            throw new Exception("Remisión no encontrada");

        var control = new ControlCalidad
        {
            RemisionId = dto.RemisionId,
            FechaControl = DateTime.Now,
            CantidadImperfectos = dto.CantidadImperfectos,
            CantidadArreglos = dto.CantidadArreglos,
            CantidadPendientes = dto.CantidadPendientes,
            CantidadAprobados = dto.CantidadAprobados,
            CausaImperfecto = dto.CausaImperfecto,
            Observaciones = dto.Observaciones,
            EstadoArreglos = dto.CantidadArreglos > 0 ? "Pendiente" : "N/A",
            RevisadoPor = dto.RevisadoPor
        };

        await _controlCalidadRepository.AddAsync(control);
        await _unitOfWork.CompleteAsync();

        // Enviar notificación si hay imperfectos
        if (control.CantidadImperfectos > 0)
        {
            await _notificacionService.EnviarNotificacionImperfectosAsync(control.Id);
        }

        // Agregar detalles de imperfectos
        if (dto.DetallesImperfectos != null && dto.DetallesImperfectos.Any())
        {
            foreach (var detalle in dto.DetallesImperfectos)
            {
                await _context.DetallesImperfectos.AddAsync(new DetalleImperfecto
                {
                    ControlCalidadId = control.Id,
                    TipoDefecto = detalle.TipoDefecto,
                    Descripcion = detalle.Descripcion,
                    Cantidad = detalle.Cantidad
                });
            }
            await _unitOfWork.CompleteAsync();
        }

        return await GetControlByIdAsync(control.Id) ?? throw new Exception("Error al crear el control de calidad");
    }

    public async Task<ControlCalidadDto?> GetControlByIdAsync(int id)
    {
        var control = await _context.ControlesCalidad
            .Include(c => c.Remision)
                .ThenInclude(r => r.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .Include(c => c.DetallesImperfectos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (control == null) return null;

        return MapToDto(control);
    }

    public async Task<IEnumerable<ControlCalidadDto>> GetControlesByRemisionAsync(int remisionId)
    {
        var controles = await _context.ControlesCalidad
            .Where(c => c.RemisionId == remisionId)
            .Include(c => c.Remision)
                .ThenInclude(r => r.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .Include(c => c.DetallesImperfectos)
            .OrderByDescending(c => c.FechaControl)
            .ToListAsync();

        return controles.Select(MapToDto).ToList();
    }

    public async Task<ControlCalidadDto> ActualizarEstadoArreglosAsync(int id, string estado)
    {
        var control = await _controlCalidadRepository.GetByIdAsync(id);
        if (control == null)
            throw new Exception("Control de calidad no encontrado");

        var estadosValidos = new[] { "Pendiente", "En Proceso", "Listo", "Devuelto", "Rechazado", "N/A" };
        if (!estadosValidos.Contains(estado))
            throw new Exception("Estado de arreglos no válido");

        control.EstadoArreglos = estado;
        await _unitOfWork.CompleteAsync();

        return await GetControlByIdAsync(id) ?? throw new Exception("Error al actualizar el estado");
    }

    private ControlCalidadDto MapToDto(ControlCalidad control)
    {
        return new ControlCalidadDto
        {
            Id = control.Id,
            RemisionId = control.RemisionId,
            NumeroRemision = control.Remision?.NumeroRemision,
            TallerNombre = control.Remision?.AsignacionTaller?.Taller?.Nombre,
            FechaControl = control.FechaControl,
            CantidadImperfectos = control.CantidadImperfectos,
            CantidadArreglos = control.CantidadArreglos,
            CantidadPendientes = control.CantidadPendientes,
            CantidadAprobados = control.CantidadAprobados,
            CausaImperfecto = control.CausaImperfecto,
            Observaciones = control.Observaciones,
            EstadoArreglos = control.EstadoArreglos,
            RevisadoPor = control.RevisadoPor,
            DetallesImperfectos = control.DetallesImperfectos?.Select(d => new DetalleImperfectoDto
            {
                TipoDefecto = d.TipoDefecto,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad
            }).ToList() ?? new List<DetalleImperfectoDto>()
        };
    }
}
