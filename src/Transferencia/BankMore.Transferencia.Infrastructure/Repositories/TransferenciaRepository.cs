using BankMore.Transferencia.Domain.Interfaces;
using BankMore.Transferencia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.Transferencia.Infrastructure.Repositories;

public class TransferenciaRepository : ITransferenciaRepository
{
    private readonly TransferenciaContext _context;

    public TransferenciaRepository(TransferenciaContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Transferencia?> GetByIdAsync(string idTransferencia) =>
        await _context.Transferencias.FindAsync(idTransferencia);

    public async Task<IEnumerable<Domain.Entities.Transferencia>> GetAllAsync() =>
        await _context.Transferencias.ToListAsync();

    public async Task AddAsync(Domain.Entities.Transferencia transferencia) =>
        await _context.Transferencias.AddAsync(transferencia);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task<IEnumerable<Domain.Entities.Transferencia>> GetByContaAsync(string idConta)
    {
        return await _context.Transferencias
            .Where(t => t.IdContaOrigem == idConta || t.IdContaDestino == idConta)
            .ToListAsync();
    }
}
