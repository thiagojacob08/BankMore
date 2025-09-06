using BankMore.Transferencia.Application.DTOs;
using BankMore.Transferencia.Application.Queries;
using BankMore.Transferencia.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.Transferencia.Application.Handlers
{
    public class ObterTransferenciasPorContaHandler : IRequestHandler<ObterTransferenciasPorContaQuery, IEnumerable<TransferenciaDto>>
    {
        private readonly ITransferenciaRepository _repository;

        public ObterTransferenciasPorContaHandler(ITransferenciaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TransferenciaDto>> Handle(ObterTransferenciasPorContaQuery request, CancellationToken cancellationToken)
        {
            var transferencias = await _repository.GetByContaAsync(request.IdConta);
            return transferencias.Select(t => new TransferenciaDto
            {
                IdTransferencia = t.IdTransferencia,
                IdContaOrigem = t.IdContaOrigem,
                IdContaDestino = t.IdContaDestino,
                Valor = t.Valor,
                DataMovimento = t.DataMovimento
            });
        }

        public async Task<TransferenciaDto> Handle(ObterTransferenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            var transferencia = await _repository.GetByIdAsync(request.IdTransferencia);
            if (transferencia == null) return null!;

            return new TransferenciaDto
            {
                IdTransferencia = transferencia.IdTransferencia,
                IdContaOrigem = transferencia.IdContaOrigem,
                IdContaDestino = transferencia.IdContaDestino,
                Valor = transferencia.Valor,
                DataMovimento = transferencia.DataMovimento
            };
        }
    }
}
