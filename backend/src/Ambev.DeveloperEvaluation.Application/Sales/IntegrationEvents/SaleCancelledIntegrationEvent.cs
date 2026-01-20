using Ambev.DeveloperEvaluation.Application.Abstractions.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.IntegrationEvents;

public record SaleCancelledIntegrationEvent(
    Guid SaleId
    ) : IIntegrationEvent;
