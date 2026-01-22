using Ambev.DeveloperEvaluation.Application.Abstractions.Events;
using Ambev.DeveloperEvaluation.Application.Sales.IntegrationEvents;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DomainEventHandlers;

public class SaleCancelledEventHandler : IDomainEventHandler<SaleCancelledDomainEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;
    private readonly IIntegrationEventsDispatcher _dispatcher;

    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger, IIntegrationEventsDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }
    
    public async Task Handle(SaleCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale cancelled: {SaleId}", domainEvent.SaleId);

        var integrationEvent = new SaleCancelledIntegrationEvent(domainEvent.SaleId);
        
        await _dispatcher.DispatchAsync(integrationEvent, cancellationToken);
    }
}