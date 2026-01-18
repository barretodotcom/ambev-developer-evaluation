namespace Ambev.DeveloperEvaluation.Application.Abstractions;

public interface IIntegrationEvent
{
    public Guid EventId { get; }
}