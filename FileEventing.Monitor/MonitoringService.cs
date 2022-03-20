using System.Net;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using FileEventing.Monitor.Events;

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

        using var watcher = new FileSystemWatcher(@"/tmp");

        watcher.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;

        watcher.Changed += OnFileChanged;
        watcher.Created += OnFileCreated;
        watcher.Deleted += OnFileDeleted;
        watcher.Renamed += OnFileRenamed;
        watcher.Error += OnError;

        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;

        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        
        _logger.LogInformation($"Exiting {nameof(MonitoringService)}");
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("File [{ChangedFilePath}] changed", e.FullPath);
        
#pragma warning disable MTA0001
        _bus.Publish(new FileChangedEvent(GetHostName(), e.FullPath));
#pragma warning restore MTA0001
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("File [{ChangedFilePath}] created", e.FullPath);
        
#pragma warning disable MTA0001
        _bus.Publish(new FileCreatedEvent(GetHostName(), e.FullPath));
#pragma warning restore MTA0001
    }

    private void OnFileDeleted(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("File [{ChangedFilePath}] deleted", e.FullPath);
        
#pragma warning disable MTA0001
        _bus.Publish(new FileDeletedEvent(GetHostName(), e.FullPath));
#pragma warning restore MTA0001
    }
    
    private void OnFileRenamed(object sender, FileSystemEventArgs e)
    {
        var renamed = e as RenamedEventArgs ?? throw new InvalidCastException($"Cannot cast event to {nameof(RenamedEventArgs)}");
        _logger.LogInformation("File [{ChangedFilePath}] renamed to [{NewFilePath}]", renamed.OldFullPath, renamed.FullPath);
        
#pragma warning disable MTA0001
        _bus.Publish(new FileRenamedEvent(GetHostName(), renamed.OldFullPath, renamed.FullPath));
#pragma warning restore MTA0001
    }
    
    private void OnError(object sender, ErrorEventArgs e)
    {
        _logger.LogError(e.GetException(), "Watcher exception");
    }

    private static string GetHostName() => Dns.GetHostName();
}