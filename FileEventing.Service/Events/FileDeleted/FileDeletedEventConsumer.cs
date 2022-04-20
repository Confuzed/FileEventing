using System;
using System.Threading.Tasks;
using FileEventing.Contract.Events;
using FileEventing.Service.Data;
using FileEventing.Service.Data.Model;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileDeleted;

public class FileDeletedEventConsumer : IConsumer<IFileChangedEvent>
{
    private readonly ILogger<FileDeletedEventConsumer> _logger;
    private readonly IFileEventWriter _eventWriter;

    public FileDeletedEventConsumer(
        ILogger<FileDeletedEventConsumer> logger,
        IFileEventWriter eventWriter)
    {
        _logger = logger;
        _eventWriter = eventWriter;
    }
    
    public async Task Consume(ConsumeContext<IFileChangedEvent> context)
    {
        var fileEvent = context.Message;
        
        _logger.LogInformation("Received FileDeleted event for {ModifiedFileHost}:{ModifiedFilePath}",
            fileEvent.Host,
            fileEvent.Path);

        await _eventWriter.WriteAsync(
            new File(fileEvent.Host, fileEvent.Path, EventType.Deleted)
            {
                Size = 0,
            },
            context.CancellationToken);
    }

}