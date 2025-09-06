using BankMore.Transferencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Transferencia.Infrastructure.Data;

public class TransferenciaContext : DbContext
{
    public TransferenciaContext(DbContextOptions<TransferenciaContext> options)
        : base(options) { }

    public DbSet<Domain.Entities.Transferencia> Transferencias { get; set; } = null!;
    public DbSet<Idempotencia> Idempotencias { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Transferencia>()
            .HasKey(t => t.IdTransferencia);

        modelBuilder.Entity<Idempotencia>()
            .HasKey(i => i.ChaveIdempotencia);
    }
}
