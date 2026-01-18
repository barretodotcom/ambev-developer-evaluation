namespace Ambev.DeveloperEvaluation.Application.Abstractions;

public interface IIntegrationEventDispatcher
{
    Task DispatchAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}