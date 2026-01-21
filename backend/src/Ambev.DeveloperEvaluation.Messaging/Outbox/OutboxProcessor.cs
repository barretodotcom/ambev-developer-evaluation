using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Messaging.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly int _batchSize;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _batchSize = configuration.GetValue<int>("Outbox:BatchSize");
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox processor started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox messages.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task ProcessAsync(CancellationToken cancellationToken)
    {
        
        using var scope = _scopeFactory.CreateScope();

        var transactionManager = scope.ServiceProvider.GetRequiredService<ITransactionManager>();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var publisher = scope.ServiceProvider.GetRequiredService<IEventBusPublisher>();


        await transactionManager.ExecuteAsync(async (ct) =>
        {
            var messages = await outboxRepository.GetUnprocessedAsync(_batchSize, cancellationToken);

            if (!messages.Any())
                return;

            foreach (var message in messages)
            {
                await publisher.PublishAsync(message, cancellationToken);
                message.MarkAsProcessed();
            }

        }, cancellationToken);
        
    }
}