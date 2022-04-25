using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileEventing.Monitor;

public class MonitoringService : BackgroundService
{
    private readonly ILogger<MonitoringService> _logger;
    private readonly IBus _bus;
    private readonly IFileAccess _fileAccess;

    public MonitoringService(ILogger<MonitoringService> logger, IBus bus, IFileAccess fileAccess)
    {
        _logger = logger;
        _bus = bus;
        _fileAccess = fileAccess;
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

    private async void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("File [{ChangedFilePath}] changed", e.FullPath);
        await _bus.PublishFileChanged(e.FullPath, _fileAccess.GetLength(e.FullPath));
    }

    private async void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("File [{ChangedFilePath}] created", e.FullPath);
        await _bus.PublishFileCreated(e.FullPath, _fileAccess.GetLength(e.FullPath));
    }

    private async void OnFileDeleted(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("File [{ChangedFilePath}] deleted", e.FullPath);
        await _bus.PublishFileDeleted(e.FullPath);
    }
    
    private async void OnFileRenamed(object sender, FileSystemEventArgs e)
    {
        var renamed = e as RenamedEventArgs ?? throw new InvalidCastException($"Cannot cast event to {nameof(RenamedEventArgs)}");
        _logger.LogInformation("File [{ChangedFilePath}] renamed to [{NewFilePath}]", renamed.OldFullPath, renamed.FullPath);
        await _bus.PublishFileRenamed(renamed.OldFullPath, renamed.FullPath);
    }
    
    private void OnError(object sender, ErrorEventArgs e)
    {
        _logger.LogError(e.GetException(), "Monitor exception");
    }
}