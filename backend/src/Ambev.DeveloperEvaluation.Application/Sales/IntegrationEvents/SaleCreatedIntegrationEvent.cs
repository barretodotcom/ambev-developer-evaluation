using Ambev.DeveloperEvaluation.Application.Abstractions;

namespace Ambev.DeveloperEvaluation.Application.Sales.IntegrationEvents;

public record SaleCreatedIntegrationEvent(
    Guid EventId,
    Guid SaleId
    ) : IIntegrationEvent;