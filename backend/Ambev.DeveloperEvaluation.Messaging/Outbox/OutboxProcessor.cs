using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Messaging.Outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly ILogger _logger;

    public OutboxProcessor(ILogger<OutboxProcessor> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox processor is starting.");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Outbox processing started on: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

}