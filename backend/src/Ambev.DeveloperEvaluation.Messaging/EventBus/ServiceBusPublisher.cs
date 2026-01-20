using System.Text;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Messaging.Outbox;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.Messaging.EventBus;

/// <summary>
/// Responsible for publishing integration events to Azure Service Bus.
/// </summary>
/// <remarks>
/// This publisher acts as an abstraction over Azure Service Bus,
/// decoupling the domain/application layer from the messaging infrastructure.
///
/// It is commonly used in conjunction with the OutboxEntity Pattern,
/// where events are persisted first and published asynchronously
/// to guarantee reliability and eventual consistency.
///
/// This component should be implemented in the Infrastructure layer
/// and injected via dependency inversion.
/// </remarks>
public class ServiceBusPublisher : IEventBusPublisher
{
    private readonly ServiceBusSender _sender;
    
    public ServiceBusPublisher(ServiceBusClient client, IConfiguration configuration)
    {
        var topicName = configuration.GetValue<string>("Azure:ServiceBus:TopicName");
        _sender = client.CreateSender(topicName);
    }
    /// <summary>
    /// Publishes an outbox message to Azure Service Bus.
    /// </summary>
    /// <param name="message">
    /// The outbox record containing the serialized event payload.
    /// </param>
    /// <param name="cancellationToken">
    /// Token to cancel the publish operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous publish operation.
    /// </returns>
    /// <remarks>
    /// This method sends a previously persisted outbox message to Azure Service Bus,
    /// ensuring reliable integration event delivery and supporting idempotency.
    /// The publisher contains no domain logic and belongs to the infrastructure layer.
    /// </remarks>
    public async Task PublishAsync(OutboxEntity message,CancellationToken cancellationToken)
    {
        var serviceBusMessage = new ServiceBusMessage(
            Encoding.UTF8.GetBytes(message.Payload))
        {
            MessageId = message.Id.ToString(),
            ContentType = "application/json",
            Subject = message.Type
        };

        serviceBusMessage.ApplicationProperties["event_type"] = message.Type;
        serviceBusMessage.ApplicationProperties["occurred_on"] = message.OccurredOn;

        await _sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}