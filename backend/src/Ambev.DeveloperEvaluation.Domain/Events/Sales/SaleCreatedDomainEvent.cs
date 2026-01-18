namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public sealed record SaleCreatedDomainEvent(
    Guid SaleId,
    Guid CustomerId,
    DateTime SaleDate
    ) : IDomainEvent;