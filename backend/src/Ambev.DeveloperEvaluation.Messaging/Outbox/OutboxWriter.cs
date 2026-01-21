using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Abstractions.Events;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public class OutboxWriter : IIntegrationEventsDispatcher
{
    private readonly IOutboxRepository _outboxRepository;

    public OutboxWriter(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }
    
    public async Task DispatchAsync(IIntegrationEvent message, CancellationToken cancellationToken)
    {
        var type = message.GetType().Name;
        
        var outbox = new OutboxEntity(
            type,
            JsonSerializer.Serialize(message, message.GetType()),
            DateTime.UtcNow
            );
        
        await _outboxRepository.CreateAsync(outbox, cancellationToken);
    }
}