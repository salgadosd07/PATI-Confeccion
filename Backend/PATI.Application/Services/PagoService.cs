using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface IPagoService
{
    Task<PagoDto> CreatePagoAsync(CreatePagoDto dto);
    Task<PagoDto?> GetPagoByIdAsync(int id);
    Task<IEnumerable<PagoDto>> GetPagosByAsignacionAsync(int asignacionId);
    Task<PagoDto> ActualizarEstadoPagoAsync(int id, ActualizarEstadoPagoDto dto);
    Task<IEnumerable<PagoDto>> GetPagosPendientesAsync();
    Task<decimal> CalcularTotalPagadoByTallerAsync(int tallerId);
}

public class PagoService : IPagoService
{
    private readonly IRepository<Pago> _pagoRepository;
    private readonly IRepository<AsignacionTaller> _asignacionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly INotificacionService _notificacionService;

    public PagoService(
        IRepository<Pago> pagoRepository,
        IRepository<AsignacionTaller> asignacionRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context,
        INotificacionService notificacionService)
    {
        _pagoRepository = pagoRepository;
        _asignacionRepository = asignacionRepository;
        _unitOfWork = unitOfWork;
        _context = context;
        _notificacionService = notificacionService;
    }

    public async Task<PagoDto> CreatePagoAsync(CreatePagoDto dto)
    {
        var asignacion = await _context.AsignacionesTaller
            .Include(a => a.Taller)
            .FirstOrDefaultAsync(a => a.Id == dto.AsignacionTallerId);
        
        if (asignacion == null)
            throw new Exception("Asignación no encontrada");

        var numeroPago = GenerarNumeroPago();

        var pago = new Pago
        {
            NumeroPago = numeroPago,
            AsignacionTallerId = dto.AsignacionTallerId,
            FechaPago = DateTime.Now,
            MontoTotal = dto.MontoTotal,
            MontoPagado = dto.MontoPagado ?? 0,
            EstadoPago = dto.MontoPagado >= dto.MontoTotal ? "Pagado" : 
                        dto.MontoPagado > 0 ? "Parcial" : "Pendiente",
            MetodoPago = dto.MetodoPago,
            Referencia = dto.Referencia,
            Observaciones = dto.Observaciones
        };

        // Calcular días de mora si hay fecha estimada
        if (asignacion.FechaEstimadaEntrega.HasValue && DateTime.Now > asignacion.FechaEstimadaEntrega.Value)
        {
            pago.DiasMora = (int)(DateTime.Now - asignacion.FechaEstimadaEntrega.Value).TotalDays;
        }

        await _pagoRepository.AddAsync(pago);
        await _unitOfWork.CompleteAsync();

        // Enviar notificación de pago
        await _notificacionService.EnviarNotificacionPagoAsync(pago.Id, pago.EstadoPago);

        return await GetPagoByIdAsync(pago.Id) ?? throw new Exception("Error al crear el pago");
    }

    public async Task<PagoDto?> GetPagoByIdAsync(int id)
    {
        var pago = await _context.Pagos
            .Include(p => p.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .Include(p => p.AsignacionTaller)
                .ThenInclude(at => at.Referencia)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pago == null) return null;

        return MapToDto(pago);
    }

    public async Task<IEnumerable<PagoDto>> GetPagosByAsignacionAsync(int asignacionId)
    {
        var pagos = await _context.Pagos
            .Where(p => p.AsignacionTallerId == asignacionId)
            .Include(p => p.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();

        return pagos.Select(MapToDto).ToList();
    }

    public async Task<PagoDto> ActualizarEstadoPagoAsync(int id, ActualizarEstadoPagoDto dto)
    {
        var pago = await _pagoRepository.GetByIdAsync(id);
        if (pago == null)
            throw new Exception("Pago no encontrado");

        pago.MontoPagado = dto.MontoPagado;
        pago.EstadoPago = dto.EstadoPago;
        pago.MetodoPago = dto.MetodoPago;
        pago.Observaciones = dto.Observaciones;

        await _unitOfWork.CompleteAsync();

        return await GetPagoByIdAsync(id) ?? throw new Exception("Error al actualizar el pago");
    }

    public async Task<IEnumerable<PagoDto>> GetPagosPendientesAsync()
    {
        var pagos = await _context.Pagos
            .Where(p => p.EstadoPago == "Pendiente" || p.EstadoPago == "Parcial")
            .Include(p => p.AsignacionTaller)
                .ThenInclude(at => at.Taller)
            .Include(p => p.AsignacionTaller)
                .ThenInclude(at => at.Referencia)
            .OrderBy(p => p.FechaPago)
            .ToListAsync();

        return pagos.Select(MapToDto).ToList();
    }

    public async Task<decimal> CalcularTotalPagadoByTallerAsync(int tallerId)
    {
        var total = await _context.Pagos
            .Where(p => p.AsignacionTaller.TallerId == tallerId && p.EstadoPago == "Pagado")
            .SumAsync(p => p.MontoPagado ?? 0);

        return total;
    }

    private PagoDto MapToDto(Pago pago)
    {
        return new PagoDto
        {
            Id = pago.Id,
            NumeroPago = pago.NumeroPago,
            AsignacionTallerId = pago.AsignacionTallerId,
            TallerNombre = pago.AsignacionTaller?.Taller?.Nombre,
            ReferenciaNombre = pago.AsignacionTaller?.Referencia?.Nombre,
            FechaPago = pago.FechaPago,
            MontoTotal = pago.MontoTotal,
            MontoPagado = pago.MontoPagado,
            EstadoPago = pago.EstadoPago,
            MetodoPago = pago.MetodoPago,
            Referencia = pago.Referencia,
            Observaciones = pago.Observaciones,
            DiasMora = pago.DiasMora
        };
    }

    private string GenerarNumeroPago()
    {
        return $"PAG-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
