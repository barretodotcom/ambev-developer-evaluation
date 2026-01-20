using Ambev.DeveloperEvaluation.Application.Abstractions.Events;
using Ambev.DeveloperEvaluation.Application.Sales.IntegrationEvents;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DomainEventHandlers;

public class SaleCreatedEventHandler : IDomainEventHandler<SaleCreatedDomainEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    private readonly IIntegrationEventsDispatcher _dispatcher;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger, IIntegrationEventsDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    public async Task Handle(SaleCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale Created Event: {DomainEvent}", domainEvent);

        var integrationEvent = new SaleCreatedIntegrationEvent(domainEvent.SaleId);

        await _dispatcher.DispatchAsync(integrationEvent, cancellationToken);
    }
}