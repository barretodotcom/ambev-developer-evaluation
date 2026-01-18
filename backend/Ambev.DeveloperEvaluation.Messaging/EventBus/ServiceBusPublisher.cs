using Ambev.DeveloperEvaluation.Application.Abstractions;

namespace Ambev.DeveloperEvaluation.Messaging.EventBus;

public class ServiceBusPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }

}