namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public record SaleUpdatedDomainEvent(Guid Id, Guid CustomerId) : IDomainEvent;