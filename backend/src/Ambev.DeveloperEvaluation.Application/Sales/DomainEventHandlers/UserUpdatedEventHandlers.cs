using Ambev.DeveloperEvaluation.Application.Abstractions.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Users;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.DomainEventHandlers;

public class UserUpdatedEventHandlers : IDomainEventHandler<UserUpdatedDomainEvent>
{
    private readonly ISaleRepository _saleRepository;

    public UserUpdatedEventHandlers(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task Handle(UserUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetByCustomerIdAsync(domainEvent.Id, cancellationToken);
        foreach (var sale in sales)
            sale.UpdateCustomerName(domainEvent.Username);
    }
}