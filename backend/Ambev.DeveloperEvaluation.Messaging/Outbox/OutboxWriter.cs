using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public class OutboxWriter : IIntegrationEventDispatcher
{
    private readonly DbSet<Outbox> _outbox;

    public OutboxWriter(DbContext context)
    {
        _outbox = context.Set<Outbox>();
    }
    
    public async Task DispatchAsync(IIntegrationEvent message, CancellationToken cancellationToken)
    {
        var type = message.GetType().Name;
        
        var outbox = new Outbox(
            type,
            JsonSerializer.Serialize(message),
            DateTime.UtcNow
            );
        
        await _outbox.AddAsync(outbox, cancellationToken);
    }
}