using FileEventing.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Logging;

public class FileEventLoggingConsumer : IConsumer<FileModifiedEvent>
{
    private readonly ILogger<FileEventLoggingConsumer> _logger;

    public FileEventLoggingConsumer(ILogger<FileEventLoggingConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<FileModifiedEvent> context)
    {
        var modifiedFileEvent = context.Message;
        
        _logger.LogInformation("Modified file event received for {FilePath}", modifiedFileEvent.Path);
        
        return Task.CompletedTask;
    }
}