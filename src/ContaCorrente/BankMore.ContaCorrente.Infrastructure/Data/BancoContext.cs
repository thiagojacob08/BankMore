using BankMore.ContaCorrente.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Infrastructure.Data;

public class BancoContext : DbContext
{
    public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

    public DbSet<Domain.Entities.ContaCorrente> ContasCorrentes { get; set; } = null!;
    public DbSet<Movimento> Movimentos { get; set; } = null!;
    public DbSet<Transferencia> Transferencias { get; set; } = null!;
    public DbSet<Tarifa> Tarifas { get; set; } = null!;
    public DbSet<Idempotencia> Idempotencias { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ContaCorrente
        modelBuilder.Entity<Domain.Entities.ContaCorrente>()
            .HasKey(c => c.IdContaCorrente);

        modelBuilder.Entity<Domain.Entities.ContaCorrente>()
            .HasIndex(c => c.Numero)
            .IsUnique();

        modelBuilder.Entity<Domain.Entities.ContaCorrente>()
            .Property(c => c.Ativo);

        // Movimento
        modelBuilder.Entity<Movimento>()
            .HasKey(m => m.IdMovimento);

        modelBuilder.Entity<Movimento>()
            .HasOne(m => m.ContaCorrente)
            .WithMany(c => c.Movimentos)
            .HasForeignKey(m => m.IdContaCorrente)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movimento>()
            .Property(m => m.TipoMovimento)
            .HasConversion<string>()
            .HasMaxLength(1);

        // Transferencia
        modelBuilder.Entity<Transferencia>()
            .HasKey(t => t.IdTransferencia);

        modelBuilder.Entity<Transferencia>()
            .HasOne(t => t.ContaOrigem)
            .WithMany(c => c.TransferenciasOrigem)
            .HasForeignKey(t => t.IdContaCorrenteOrigem)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transferencia>()
            .HasOne(t => t.ContaDestino)
            .WithMany(c => c.TransferenciasDestino)
            .HasForeignKey(t => t.IdContaCorrenteDestino)
            .OnDelete(DeleteBehavior.Restrict);

        // Tarifa
        modelBuilder.Entity<Tarifa>()
            .HasKey(t => t.IdTarifa);

        modelBuilder.Entity<Tarifa>()
            .HasOne(t => t.ContaCorrente)
            .WithMany(c => c.Tarifas)
            .HasForeignKey(t => t.IdContaCorrente)
            .OnDelete(DeleteBehavior.Cascade);

        // Idempotencia
        modelBuilder.Entity<Idempotencia>()
            .HasKey(i => i.ChaveIdempotencia);
    }
}
