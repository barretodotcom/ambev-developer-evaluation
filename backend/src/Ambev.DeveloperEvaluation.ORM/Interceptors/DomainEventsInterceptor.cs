using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ambev.DeveloperEvaluation.ORM.Interceptors;

public class DomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(eventData, cancellationToken);
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContextEventData eventData, CancellationToken cancellationToken)
    {
        if (eventData.Context == null)
            return;

        var aggregates = eventData.Context.ChangeTracker.Entries<AggregateRoot>().Select(l => l.Entity).ToList();

        var domainEvents = aggregates.SelectMany(l => l.DomainEvents);
        
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
        
        foreach (var entity in aggregates)
        {
            entity.ClearDomainEvents();
        }
        
    }
    
}