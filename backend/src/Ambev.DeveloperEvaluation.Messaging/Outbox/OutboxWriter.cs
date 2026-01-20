using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Abstractions.Events;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public class OutboxWriter : IIntegrationEventsDispatcher
{
    private readonly DbSet<OutboxEntity> _outbox;

    public OutboxWriter(DbContext context)
    {
        _outbox = context.Set<OutboxEntity>();
    }
    
    public async Task DispatchAsync(IIntegrationEvent message, CancellationToken cancellationToken)
    {
        var type = message.GetType().Name;
        
        var outbox = new OutboxEntity(
            type,
            JsonSerializer.Serialize(message, message.GetType()),
            DateTime.UtcNow
            );
        
        await _outbox.AddAsync(outbox, cancellationToken);
    }
}