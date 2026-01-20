namespace Ambev.DeveloperEvaluation.Application.Abstractions.Events;

public interface IIntegrationEventsDispatcher
{
    Task DispatchAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}