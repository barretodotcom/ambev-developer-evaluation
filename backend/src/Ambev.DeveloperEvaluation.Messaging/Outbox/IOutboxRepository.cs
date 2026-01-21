namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public interface IOutboxRepository
{
    Task<IReadOnlyList<OutboxEntity>> GetUnprocessedAsync(
        int batchSize,
        CancellationToken cancellationToken);
    Task CreateAsync(OutboxEntity outbox, CancellationToken cancellationToken);
}