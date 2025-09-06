using BankMore.ContaCorrente.Application.DTOs;
using BankMore.ContaCorrente.Application.Queries;
using BankMore.ContaCorrente.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Application.Handlers
{
    public class ObterContaPorNumeroHandler : IRequestHandler<ObterContaPorNumeroQuery, ContaDetalhesDto>
    {
        private readonly BancoContext _context;

        public ObterContaPorNumeroHandler(BancoContext context)
        {
            _context = context;
        }

        public async Task<ContaDetalhesDto> Handle(ObterContaPorNumeroQuery request, CancellationToken cancellationToken)
        {
            var conta = await _context.ContasCorrentes
                .FirstOrDefaultAsync(c => c.Numero == request.numeroContaCorrente, cancellationToken);

            if (conta == null)
                throw new InvalidOperationException("Conta não cadastrada.");

            return new ContaDetalhesDto
            {
                NumeroConta = conta.Numero,
                Nome = conta.Nome,
                Ativo = conta.Ativo,
                IdContaCorrente = conta.IdContaCorrente
            };
        }
    }
}
