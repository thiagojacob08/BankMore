using BankMore.Transferencia.Domain.Entities;
using ServiceStack.Messaging;

namespace BankMore.Transferencia.Application.Services;

public class TransferenciaEventProducer : ITransferenciaEventProducer
{
    private readonly IMessageProducerAccessor _producerAccessor;

    public TransferenciaEventProducer(IMessageProducerAccessor producerAccessor)
    {
        _producerAccessor = producerAccessor;
    }

    public Task PublishAsync(TransferenciaRealizadaEvent evt)
    {
        // Obter o producer pelo nome do tópico
        var producer = _producerAccessor.GetProducer("transferencias-realizadas");

        // Produzir mensagem assincronamente
        return producer.ProduceAsync(evt);
    }
}
