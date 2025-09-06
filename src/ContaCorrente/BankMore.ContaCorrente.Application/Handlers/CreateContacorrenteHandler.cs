using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace BankMore.ContaCorrente.Application.Handlers
{
    public class CreateContaCorrenteHandler : IRequestHandler<CreateContaCorrenteCommand, CreateContaCorrenteResult>
    {
        private readonly IContaCorrenteRepository _repository;

        public CreateContaCorrenteHandler(IContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateContaCorrenteResult> Handle(CreateContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            // Gerar salt e hash da senha
            var salt = GenerateSalt();
            var senhaHash = HashPassword(request.Senha, salt);

            // Gerar número da conta
            int numeroConta;
            do
            {
                numeroConta = new Random().Next(100000, 999999);
            } while (await _repository.GetByNumeroAsync(numeroConta) != null);

            var conta = new Domain.Entities.ContaCorrente
            {
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = request.Nome,
                Numero = numeroConta,
                Senha = senhaHash,
                Salt = salt,
                Ativo = true
            };

            await _repository.AddAsync(conta);
            await _repository.SaveChangesAsync();

            return new CreateContaCorrenteResult
            {
                NumeroConta = numeroConta,
                IdContaCorrente = conta.IdContaCorrente
            };
        }

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
}
