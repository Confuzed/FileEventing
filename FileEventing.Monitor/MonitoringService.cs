using FileEventing.Abstractions;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileEventing.Monitor;

public class MonitoringService : BackgroundService
{
    private readonly ILogger<MonitoringService> _logger;
    private readonly IBus _bus;

    public MonitoringService(ILogger<MonitoringService> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Starting {nameof(MonitoringService)}");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Sending modified file event...");
            await _bus.Publish(new FileModifiedEvent("/a/b/c"), stoppingToken);
            
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
        
        _logger.LogInformation($"Exiting {nameof(MonitoringService)}");
    }
}