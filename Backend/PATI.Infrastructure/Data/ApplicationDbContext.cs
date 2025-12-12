using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PATI.Domain.Entities;

namespace PATI.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Referencia> Referencias { get; set; } = null!;
    public DbSet<Color> Colores { get; set; } = null!;
    public DbSet<Talla> Tallas { get; set; } = null!;
    public DbSet<Material> Materiales { get; set; } = null!;
    public DbSet<Corte> Cortes { get; set; } = null!;
    public DbSet<CorteColor> CorteColores { get; set; } = null!;
    public DbSet<CorteTalla> CorteTallas { get; set; } = null!;
    public DbSet<Taller> Talleres { get; set; } = null!;
    public DbSet<AsignacionTaller> AsignacionesTaller { get; set; } = null!;
    public DbSet<AvanceTaller> AvancesTaller { get; set; } = null!;
    public DbSet<Remision> Remisiones { get; set; } = null!;
    public DbSet<RemisionDetalle> RemisionDetalles { get; set; } = null!;
    public DbSet<ControlCalidad> ControlesCalidad { get; set; } = null!;
    public DbSet<DetalleImperfecto> DetallesImperfectos { get; set; } = null!;
    public DbSet<Pago> Pagos { get; set; } = null!;
    public DbSet<Inventario> Inventarios { get; set; } = null!;
    public DbSet<Notificacion> Notificaciones { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CorteColor>()
            .HasKey(cc => new { cc.CorteId, cc.ColorId });

        modelBuilder.Entity<CorteColor>()
            .HasOne(cc => cc.Corte)
            .WithMany(c => c.CorteColores)
            .HasForeignKey(cc => cc.CorteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CorteColor>()
            .HasOne(cc => cc.Color)
            .WithMany(c => c.CorteColores)
            .HasForeignKey(cc => cc.ColorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CorteTalla>()
            .HasKey(ct => new { ct.CorteId, ct.TallaId });

        modelBuilder.Entity<CorteTalla>()
            .HasOne(ct => ct.Corte)
            .WithMany(c => c.CorteTallas)
            .HasForeignKey(ct => ct.CorteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CorteTalla>()
            .HasOne(ct => ct.Talla)
            .WithMany(t => t.CorteTallas)
            .HasForeignKey(ct => ct.TallaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Corte>()
            .HasOne(c => c.Referencia)
            .WithMany(r => r.Cortes)
            .HasForeignKey(c => c.ReferenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Corte>()
            .HasOne(c => c.Material)
            .WithMany(m => m.Cortes)
            .HasForeignKey(c => c.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AsignacionTaller>()
            .HasOne(at => at.Taller)
            .WithMany(t => t.Asignaciones)
            .HasForeignKey(at => at.TallerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AsignacionTaller>()
            .HasOne(at => at.Referencia)
            .WithMany(r => r.Asignaciones)
            .HasForeignKey(at => at.ReferenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AvanceTaller>()
            .HasOne(av => av.AsignacionTaller)
            .WithMany(at => at.Avances)
            .HasForeignKey(av => av.AsignacionTallerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Remision>()
            .HasOne(r => r.AsignacionTaller)
            .WithMany(at => at.Remisiones)
            .HasForeignKey(r => r.AsignacionTallerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RemisionDetalle>()
            .HasOne(rd => rd.Remision)
            .WithMany(r => r.Detalles)
            .HasForeignKey(rd => rd.RemisionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ControlCalidad>()
            .HasOne(cc => cc.Remision)
            .WithMany(r => r.ControlesCalidad)
            .HasForeignKey(cc => cc.RemisionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleImperfecto>()
            .HasOne(di => di.ControlCalidad)
            .WithMany(cc => cc.DetallesImperfectos)
            .HasForeignKey(di => di.ControlCalidadId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.AsignacionTaller)
            .WithMany(at => at.Pagos)
            .HasForeignKey(p => p.AsignacionTallerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Referencia)
            .WithMany()
            .HasForeignKey(i => i.ReferenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Talla)
            .WithMany()
            .HasForeignKey(i => i.TallaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Color)
            .WithMany()
            .HasForeignKey(i => i.ColorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pago>()
            .Property(p => p.MontoTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Pago>()
            .Property(p => p.MontoPagado)
            .HasPrecision(18, 2);

        modelBuilder.Entity<AsignacionTaller>()
            .Property(at => at.ValorUnitario)
            .HasPrecision(18, 2);

        modelBuilder.Entity<AsignacionTaller>()
            .Property(at => at.ValorTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<AvanceTaller>()
            .Property(av => av.PorcentajeAvance)
            .HasPrecision(5, 2);

        // Configurar nombres de tablas para que coincidan con la base de datos
        modelBuilder.Entity<Inventario>().ToTable("Inventario");
        modelBuilder.Entity<Notificacion>().ToTable("Notificaciones");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaModificacion = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
