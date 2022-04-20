using System.Threading.Tasks;
using FileEventing.Contract.Events;
using FileEventing.Service.Data;
using FileEventing.Service.Data.Model;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileCreated;

public class FileCreatedEventConsumer : IConsumer<IFileCreatedEvent>
{
    private readonly ILogger<FileCreatedEventConsumer> _logger;
    private readonly IFileEventWriter _eventWriter;

    public FileCreatedEventConsumer(
        ILogger<FileCreatedEventConsumer> logger,
        IFileEventWriter eventWriter)
    {
        _logger = logger;
        _eventWriter = eventWriter;
    }
    
    public async Task Consume(ConsumeContext<IFileCreatedEvent> context)
    {
        var fileEvent = context.Message;
        
        _logger.LogInformation("Received FileCreated event for {ModifiedFileHost}:{ModifiedFilePath}",
            fileEvent.Host,
            fileEvent.Path);

        await _eventWriter.WriteAsync(
            new File(fileEvent.Host, fileEvent.Path, EventType.Created),
            context.CancellationToken);
    }
}