using HttpClientFactoryLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static BankMore.ContaCorrente.Domain.Enums.Emuns;

namespace BankMore.Transferencia.Application.Services;

public class ContaCorrenteApiService : IContaCorrenteApiService
{
    private readonly HttpClient _httpClient;

    public ContaCorrenteApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ContaCorrenteApi");
    }

    public async Task MovimentarDebitoAsync(string idConta, decimal valor, string chaveIdempotencia, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/contacorrente/movimentar", new
        {
            ChaveIdempotencia = chaveIdempotencia,
            IdContaOrigem = idConta,
            TipoMovimento = TipoMovimentacao.D,
            Valor = valor
        }, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Falha ao debitar a conta.");
    }

    public async Task MovimentarCreditoAsync(string idContaDestino, decimal valor, string chaveIdempotencia, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/contacorrente/movimentar", new
        {
            ChaveIdempotencia = chaveIdempotencia,
            IdContaDestino = idContaDestino,
            TipoMovimento = TipoMovimentacao.C,
            Valor = valor
        }, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Falha ao creditar a conta.");
    }
}
