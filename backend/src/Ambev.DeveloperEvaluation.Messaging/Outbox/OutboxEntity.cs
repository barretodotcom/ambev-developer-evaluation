namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public sealed class OutboxEntity
{
    public Guid Id { get; private set; }
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    private OutboxEntity()
    {
    }

    public OutboxEntity(string type, string payload, DateTime occurredOn)
    {
        Type = type;
        Payload = payload;
        OccurredOn = occurredOn;
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }
}