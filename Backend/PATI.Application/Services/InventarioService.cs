using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IInventarioService
{
    Task<InventarioDto> AgregarInventarioAsync(CreateInventarioDto dto);
    Task<InventarioDto?> GetInventarioByIdAsync(int id);
    Task<IEnumerable<InventarioDto>> GetInventarioDisponibleAsync();
    Task<IEnumerable<InventarioDto>> GetInventarioByReferenciaAsync(int referenciaId);
    Task ActualizarInventarioDesdeRemisionAsync(int remisionId);
}

public class InventarioService : IInventarioService
{
    private readonly IRepository<Inventario> _inventarioRepository;
    private readonly IRepository<Remision> _remisionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public InventarioService(
        IRepository<Inventario> inventarioRepository,
        IRepository<Remision> remisionRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _inventarioRepository = inventarioRepository;
        _remisionRepository = remisionRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<InventarioDto> AgregarInventarioAsync(CreateInventarioDto dto)
    {
        var inventario = new Inventario
        {
            ReferenciaId = dto.ReferenciaId,
            TallaId = dto.TallaId,
            ColorId = dto.ColorId,
            CodigoLote = dto.CodigoLote,
            CantidadDisponible = dto.CantidadDisponible,
            CantidadReservada = 0,
            FechaIngreso = DateTime.Now,
            Ubicacion = dto.Ubicacion,
            EstadoInventario = "Disponible"
        };

        await _inventarioRepository.AddAsync(inventario);
        await _unitOfWork.CompleteAsync();

        return await GetInventarioByIdAsync(inventario.Id) ?? throw new Exception("Error al crear inventario");
    }

    public async Task<InventarioDto?> GetInventarioByIdAsync(int id)
    {
        var inventario = await _context.Inventarios
            .Include(i => i.Referencia)
            .Include(i => i.Talla)
            .Include(i => i.Color)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventario == null) return null;

        return MapToDto(inventario);
    }

    public async Task<IEnumerable<InventarioDto>> GetInventarioDisponibleAsync()
    {
        try
        {
            var inventarios = await _context.Inventarios
                .Where(i => i.EstadoInventario == "Disponible" && i.CantidadDisponible > 0)
                .Include(i => i.Referencia)
                .Include(i => i.Talla)
                .Include(i => i.Color)
                .ToListAsync();

            // Ordenar en memoria después de cargar los datos
            var inventariosOrdenados = inventarios
                .OrderBy(i => i.Referencia?.Nombre ?? "")
                .ThenBy(i => i.Talla?.Nombre ?? "")
                .ToList();

            return inventariosOrdenados.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetInventarioDisponibleAsync: {ex.Message}");
            return new List<InventarioDto>();
        }
    }

    public async Task<IEnumerable<InventarioDto>> GetInventarioByReferenciaAsync(int referenciaId)
    {
        var inventarios = await _context.Inventarios
            .Where(i => i.ReferenciaId == referenciaId)
            .Include(i => i.Referencia)
            .Include(i => i.Talla)
            .Include(i => i.Color)
            .ToListAsync();

        return inventarios.Select(MapToDto).ToList();
    }

    public async Task ActualizarInventarioDesdeRemisionAsync(int remisionId)
    {
        var remision = await _context.Remisiones
            .Include(r => r.Detalles)
            .Include(r => r.AsignacionTaller)
                .ThenInclude(at => at.Referencia)
            .Include(r => r.AsignacionTaller)
                .ThenInclude(at => at.Corte)
            .FirstOrDefaultAsync(r => r.Id == remisionId);

        if (remision == null || remision.EstadoRemision != "Recibida")
            throw new Exception("Remisión no encontrada o no está recibida");

        foreach (var detalle in remision.Detalles)
        {
            var inventarioExistente = await _context.Inventarios
                .FirstOrDefaultAsync(i => 
                    i.ReferenciaId == remision.AsignacionTaller.ReferenciaId &&
                    i.TallaId == detalle.TallaId &&
                    i.ColorId == detalle.ColorId &&
                    i.CodigoLote == remision.AsignacionTaller.Corte.CodigoLote);

            if (inventarioExistente != null)
            {
                inventarioExistente.CantidadDisponible += detalle.Cantidad;
            }
            else
            {
                await _inventarioRepository.AddAsync(new Inventario
                {
                    ReferenciaId = remision.AsignacionTaller.ReferenciaId,
                    TallaId = detalle.TallaId,
                    ColorId = detalle.ColorId,
                    CodigoLote = remision.AsignacionTaller.Corte.CodigoLote,
                    CantidadDisponible = detalle.Cantidad,
                    CantidadReservada = 0,
                    FechaIngreso = DateTime.Now,
                    EstadoInventario = "Disponible"
                });
            }
        }

        await _unitOfWork.CompleteAsync();
    }

    private InventarioDto MapToDto(Inventario inventario)
    {
        return new InventarioDto
        {
            Id = inventario.Id,
            ReferenciaId = inventario.ReferenciaId,
            ReferenciaNombre = inventario.Referencia?.Nombre,
            TallaId = inventario.TallaId,
            TallaNombre = inventario.Talla?.Nombre,
            ColorId = inventario.ColorId,
            ColorNombre = inventario.Color?.Nombre,
            CodigoLote = inventario.CodigoLote,
            CantidadDisponible = inventario.CantidadDisponible,
            CantidadReservada = inventario.CantidadReservada,
            FechaIngreso = inventario.FechaIngreso,
            Ubicacion = inventario.Ubicacion,
            EstadoInventario = inventario.EstadoInventario
        };
    }
}
