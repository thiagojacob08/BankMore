using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.ContaCorrente.Application.Handlers;

public class AddTarifaHandler : IRequestHandler<AddTarifaCommand>
{
    private readonly ITarifaRepository _tarifaRepository;

    public AddTarifaHandler(ITarifaRepository tarifaRepository)
    {
        _tarifaRepository = tarifaRepository;
    }

    public async Task<Unit> Handle(AddTarifaCommand request, CancellationToken cancellationToken)
    {
        var tarifa = new Tarifa
        {
            IdTarifa = Guid.NewGuid().ToString(),
            IdContaCorrente = request.IdContaCorrente,
            Valor = request.Valor,
            DataMovimento = DateTime.UtcNow
        };

        await _tarifaRepository.AddAsync(tarifa);
        return Unit.Value;
    }

    Task IRequestHandler<AddTarifaCommand>.Handle(AddTarifaCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
