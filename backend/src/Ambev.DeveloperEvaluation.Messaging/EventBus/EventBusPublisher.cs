using Ambev.DeveloperEvaluation.Messaging.Outbox;

namespace Ambev.DeveloperEvaluation.Messaging.EventBus;

public interface IEventBusPublisher
{
    Task PublishAsync(OutboxEntity @event, CancellationToken cancellationToken);
}