using BankMore.Transferencia.Application.Commands;
using BankMore.Transferencia.Domain.Entities;
using BankMore.Transferencia.Infrastructure.Data;
using HttpClientFactoryLite;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using static BankMore.ContaCorrente.Domain.Enums.Emuns;

namespace BankMore.Transferencia.Application.Handlers
{
    public class EfetuarTransferenciaHandler : IRequestHandler<EfetuarTransferenciaCommand>
    {
        private readonly TransferenciaContext _context;
        private readonly HttpClient _httpClient;

        public EfetuarTransferenciaHandler(TransferenciaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient("ContaCorrenteApi"); // Configurado no Program.cs
        }

        public async Task<Unit> Handle(EfetuarTransferenciaCommand request, CancellationToken cancellationToken)
        {
            // ---------------- Idempotência ----------------
            var existente = await _context.Idempotencias
                .FirstOrDefaultAsync(i => i.ChaveIdempotencia == request.ChaveIdempotencia, cancellationToken);

            if (existente != null)
                return Unit.Value;

            // ---------------- Validações ----------------
            if (request.Valor <= 0)
                throw new InvalidOperationException("Valor deve ser positivo.");

            if (string.IsNullOrEmpty(request.IdContaOrigem) || string.IsNullOrEmpty(request.IdContaDestino))
                throw new InvalidOperationException("Contas inválidas.");

            // ---------------- Débito na conta origem ----------------
            var debitoResponse = await _httpClient.PostAsJsonAsync("api/contacorrente/movimentar", new
            {
                ChaveIdempotencia = request.ChaveIdempotencia,
                IdContaOrigem = request.IdContaOrigem,
                TipoMovimento = TipoMovimentacao.D,
                Valor = request.Valor
            }, cancellationToken);

            if (!debitoResponse.IsSuccessStatusCode)
                throw new InvalidOperationException("Falha ao debitar conta origem.");

            // ---------------- Crédito na conta destino ----------------
            var creditoResponse = await _httpClient.PostAsJsonAsync("api/contacorrente/movimentar", new
            {
                ChaveIdempotencia = request.ChaveIdempotencia,
                IdContaOrigem = request.IdContaOrigem, // usado apenas para token, se necessário
                IdContaDestino = request.IdContaDestino,
                TipoMovimento = TipoMovimentacao.C,
                Valor = request.Valor
            }, cancellationToken);

            if (!creditoResponse.IsSuccessStatusCode)
            {
                // ---------------- Rollback débito ----------------
                await _httpClient.PostAsJsonAsync("api/contacorrente/movimentar", new
                {
                    ChaveIdempotencia = Guid.NewGuid().ToString(),
                    IdContaOrigem = request.IdContaOrigem,
                    TipoMovimento = TipoMovimentacao.C,
                    Valor = request.Valor
                }, cancellationToken);

                throw new InvalidOperationException("Falha ao creditar conta destino. Débito revertido.");
            }

            // ---------------- Persistir Transferencia ----------------
            var transferencia = new Domain.Entities.Transferencia
            {
                IdTransferencia = Guid.NewGuid().ToString(),
                IdContaOrigem = request.IdContaOrigem,
                IdContaDestino = request.IdContaDestino,
                Valor = request.Valor,
                DataMovimento = DateTime.UtcNow
            };
            _context.Transferencias.Add(transferencia);

            // ---------------- Registrar Idempotência ----------------
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = request.ChaveIdempotencia,
                Requisicao = $"Origem:{request.IdContaOrigem},Destino:{request.IdContaDestino},Valor:{request.Valor}",
                Resultado = "Transferência realizada"
            };
            _context.Idempotencias.Add(idempotencia);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        Task IRequestHandler<EfetuarTransferenciaCommand>.Handle(EfetuarTransferenciaCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}
