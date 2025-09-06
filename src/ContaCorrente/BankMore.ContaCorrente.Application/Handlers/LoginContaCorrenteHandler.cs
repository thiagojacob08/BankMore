using BankMore.ContaCorrente.Application.Queries;
using BankMore.ContaCorrente.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankMore.ContaCorrente.Application.Handlers;

public class LoginContaCorrenteHandler : IRequestHandler<LoginContaCorrenteQuery, LoginContaCorrenteResult>
{
    private readonly BancoContext _context;
    private readonly IConfiguration _configuration;

    public LoginContaCorrenteHandler(BancoContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginContaCorrenteResult> Handle(LoginContaCorrenteQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.ContaCorrente? conta = null;

        if (!string.IsNullOrEmpty(request.CPF))
        {
            conta = await _context.ContasCorrentes.FirstOrDefaultAsync(c => c.Nome == request.CPF, cancellationToken);
        }
        else if (request.NumeroConta.HasValue)
        {
            conta = await _context.ContasCorrentes.FirstOrDefaultAsync(c => c.Numero == request.NumeroConta.Value, cancellationToken);
        }

        if (conta == null) throw new UnauthorizedAccessException("Usuário não autorizado.");

        // Verifica senha
        if (conta.Senha != HashPassword(request.Senha, conta.Salt))
        {
            throw new UnauthorizedAccessException("Usuário não autorizado.");
        }

        // Gera JWT
        var token = GenerateJwt(conta);

        return new LoginContaCorrenteResult
        {
            IdContaCorrente = conta.IdContaCorrente,
            Token = token
        };
    }

    private static string HashPassword(string senha, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(senha + salt);
        var hash = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }

    private string GenerateJwt(Domain.Entities.ContaCorrente conta)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim("id", conta.IdContaCorrente),
                new Claim("numeroConta", conta.Numero.ToString())
            };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
