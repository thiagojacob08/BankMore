using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Domain.Interfaces;
using BankMore.ContaCorrente.Infrastructure.Data;

namespace BankMore.ContaCorrente.Infrastructure.Repositories;

public class MovimentoRepository : IMovimentoRepository
{
    private readonly BancoContext _context;
    public MovimentoRepository(BancoContext context) => _context = context;

    public async Task AddAsync(Movimento movimento) =>
        await _context.Movimentos.AddAsync(movimento);
}
