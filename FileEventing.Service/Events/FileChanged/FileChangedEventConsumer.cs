using System.Threading.Tasks;
using FileEventing.Contract.Events;
using FileEventing.Service.Data;
using FileEventing.Service.Data.Model;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileChanged;

public class FileChangedEventConsumer : IConsumer<IFileChangedEvent>
{
    private readonly ILogger<FileChangedEventConsumer> _logger;
    private readonly IFileEventWriter _eventWriter;

    public FileChangedEventConsumer(
        ILogger<FileChangedEventConsumer> logger,
        IFileEventWriter eventWriter)
    {
        _logger = logger;
        _eventWriter = eventWriter;
    }
    
    public async Task Consume(ConsumeContext<IFileChangedEvent> context)
    {
        var fileEvent = context.Message;
        
        _logger.LogInformation("Received FileChanged event for {ModifiedFileHost}:{ModifiedFilePath}",
            fileEvent.Host,
            fileEvent.Path);

        await _eventWriter.WriteAsync(
            new File(fileEvent.Host, fileEvent.Path, EventType.Changed),
            context.CancellationToken);
    }
}