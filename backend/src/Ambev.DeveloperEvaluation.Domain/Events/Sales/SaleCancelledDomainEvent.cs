namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public record SaleCancelledDomainEvent(Guid SaleId) : IDomainEvent;
