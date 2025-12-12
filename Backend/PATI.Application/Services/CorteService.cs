using PATI.Application.DTOs;
using PATI.Domain.Entities;
using PATI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PATI.Infrastructure.Data;

namespace PATI.Application.Services;

public interface ICorteService
{
    Task<CorteDto> CreateCorteAsync(CreateCorteDto dto);
    Task<CorteDto?> GetCorteByIdAsync(int id);
    Task<IEnumerable<CorteDto>> GetAllCortesAsync();
    Task<CorteDto> ImportarCorteDesdeExcelAsync(string filePath);
}

public class CorteService : ICorteService
{
    private readonly IRepository<Corte> _corteRepository;
    private readonly IRepository<Referencia> _referenciaRepository;
    private readonly IRepository<Material> _materialRepository;
    private readonly IRepository<Color> _colorRepository;
    private readonly IRepository<Talla> _tallaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public CorteService(
        IRepository<Corte> corteRepository,
        IRepository<Referencia> referenciaRepository,
        IRepository<Material> materialRepository,
        IRepository<Color> colorRepository,
        IRepository<Talla> tallaRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _corteRepository = corteRepository;
        _referenciaRepository = referenciaRepository;
        _materialRepository = materialRepository;
        _colorRepository = colorRepository;
        _tallaRepository = tallaRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<CorteDto> CreateCorteAsync(CreateCorteDto dto)
    {
        var cantidadTotal = dto.Tallas.Sum(t => t.Cantidad);
        var codigoLote = GenerarCodigoLote();

        var corte = new Corte
        {
            CodigoLote = codigoLote,
            Mesa = dto.Mesa,
            FechaCorte = dto.FechaCorte,
            ReferenciaId = dto.ReferenciaId,
            MaterialId = dto.MaterialId,
            CantidadTotal = cantidadTotal,
            CantidadProgramada = dto.CantidadProgramada
        };

        await _corteRepository.AddAsync(corte);
        await _unitOfWork.CompleteAsync();

        foreach (var color in dto.Colores)
        {
            _context.CorteColores.Add(new CorteColor
            {
                CorteId = corte.Id,
                ColorId = color.ColorId,
                Cantidad = color.Cantidad
            });
        }

        foreach (var talla in dto.Tallas)
        {
            _context.CorteTallas.Add(new CorteTalla
            {
                CorteId = corte.Id,
                TallaId = talla.TallaId,
                Cantidad = talla.Cantidad
            });
        }

        await _unitOfWork.CompleteAsync();

        return await GetCorteByIdAsync(corte.Id) ?? throw new Exception("Error al crear el corte");
    }

    public async Task<CorteDto?> GetCorteByIdAsync(int id)
    {
        var corte = await _context.Cortes
            .Include(c => c.Referencia)
            .Include(c => c.Material)
            .Include(c => c.CorteColores).ThenInclude(cc => cc.Color)
            .Include(c => c.CorteTallas).ThenInclude(ct => ct.Talla)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (corte == null) return null;

        return new CorteDto
        {
            Id = corte.Id,
            CodigoLote = corte.CodigoLote,
            Mesa = corte.Mesa,
            FechaCorte = corte.FechaCorte,
            ReferenciaId = corte.ReferenciaId,
            ReferenciaNombre = corte.Referencia?.Nombre,
            MaterialId = corte.MaterialId,
            MaterialNombre = corte.Material?.Nombre,
            CantidadTotal = corte.CantidadTotal,
            CantidadProgramada = corte.CantidadProgramada,
            Colores = corte.CorteColores.Select(cc => new CorteColorDto
            {
                ColorId = cc.ColorId,
                ColorNombre = cc.Color.Nombre,
                Cantidad = cc.Cantidad
            }).ToList(),
            Tallas = corte.CorteTallas.Select(ct => new CorteTallaDto
            {
                TallaId = ct.TallaId,
                TallaNombre = ct.Talla.Nombre,
                Cantidad = ct.Cantidad
            }).ToList()
        };
    }

    public async Task<IEnumerable<CorteDto>> GetAllCortesAsync()
    {
        var cortes = await _context.Cortes
            .Include(c => c.Referencia)
            .Include(c => c.Material)
            .Include(c => c.CorteColores).ThenInclude(cc => cc.Color)
            .Include(c => c.CorteTallas).ThenInclude(ct => ct.Talla)
            .ToListAsync();

        return cortes.Select(corte => new CorteDto
        {
            Id = corte.Id,
            CodigoLote = corte.CodigoLote,
            Mesa = corte.Mesa,
            FechaCorte = corte.FechaCorte,
            ReferenciaId = corte.ReferenciaId,
            ReferenciaNombre = corte.Referencia?.Nombre,
            MaterialId = corte.MaterialId,
            MaterialNombre = corte.Material?.Nombre,
            CantidadTotal = corte.CantidadTotal,
            CantidadProgramada = corte.CantidadProgramada,
            Colores = corte.CorteColores.Select(cc => new CorteColorDto
            {
                ColorId = cc.ColorId,
                ColorNombre = cc.Color.Nombre,
                Cantidad = cc.Cantidad
            }).ToList(),
            Tallas = corte.CorteTallas.Select(ct => new CorteTallaDto
            {
                TallaId = ct.TallaId,
                TallaNombre = ct.Talla.Nombre,
                Cantidad = ct.Cantidad
            }).ToList()
        });
    }

    public Task<CorteDto> ImportarCorteDesdeExcelAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    private string GenerarCodigoLote()
    {
        return $"LOTE-{DateTime.Now:yyyyMMddHHmmss}";
    }
}
