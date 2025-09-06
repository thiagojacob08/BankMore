using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BankMore.ContaCorrente.Application.Handlers;

public class CreateContaCorrenteHandler : IRequestHandler<CreateContaCorrenteCommand, CreateContaCorrenteResult>
{
    private readonly BancoContext _context;

    public CreateContaCorrenteHandler(BancoContext context)
    {
        _context = context;
    }

    public async Task<CreateContaCorrenteResult> Handle(CreateContaCorrenteCommand request, CancellationToken cancellationToken)
    {
        // Gerar salt e hash da senha
        var salt = GenerateSalt();
        var senhaHash = HashPassword(request.Senha, salt);

        // Gerar número da conta (único)
        int numeroConta;
        do
        {
            numeroConta = new Random().Next(100000, 999999);
        } while (await _context.ContasCorrentes.AnyAsync(c => c.Numero == numeroConta));

        var conta = new Domain.Entities.ContaCorrente
        {
            IdContaCorrente = Guid.NewGuid().ToString(),
            Nome = request.Nome,
            Numero = numeroConta,
            Senha = senhaHash,
            Salt = salt,
            Ativo = true
        };

        _context.ContasCorrentes.Add(conta);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateContaCorrenteResult
        {
            NumeroConta = numeroConta,
            IdContaCorrente = conta.IdContaCorrente
        };
    }

    // ------------------ Helpers ------------------
    private static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    private static string HashPassword(string senha, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(senha + salt);
        var hash = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }
}
