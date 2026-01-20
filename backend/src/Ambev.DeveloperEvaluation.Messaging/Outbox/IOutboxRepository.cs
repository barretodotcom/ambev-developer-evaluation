namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public interface IOutboxRepository
{
    Task<IReadOnlyList<OutboxEntity>> GetUnprocessedAsync(
        int batchSize,
        CancellationToken cancellationToken);

    Task MarkAsProcessedAsync(
        IEnumerable<OutboxEntity> messages,
        CancellationToken cancellationToken);
}