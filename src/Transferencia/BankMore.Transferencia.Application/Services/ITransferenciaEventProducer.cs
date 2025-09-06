using BankMore.Transferencia.Domain.Entities;

namespace BankMore.Transferencia.Application.Services;

public interface ITransferenciaEventProducer
{
    Task PublishAsync(TransferenciaRealizadaEvent evt);
}
