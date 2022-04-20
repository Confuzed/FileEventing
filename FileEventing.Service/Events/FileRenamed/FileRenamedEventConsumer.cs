using System;
using System.Threading.Tasks;
using FileEventing.Contract.Events;
using FileEventing.Service.Data;
using FileEventing.Service.Data.Model;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileRenamed;

public class FileRenamedEventConsumer : IConsumer<IFileRenamedEvent>
{
    private readonly ILogger<FileRenamedEventConsumer> _logger;
    private readonly IFileEventWriter _eventWriter;

    public FileRenamedEventConsumer(
        ILogger<FileRenamedEventConsumer> logger,
        IFileEventWriter eventWriter)
    {
        _logger = logger;
        _eventWriter = eventWriter;
    }
    
    public async Task Consume(ConsumeContext<IFileRenamedEvent> context)
    {
        var fileEvent = context.Message;
        
        _logger.LogInformation(
            "Received FileRenamed event; {ModifiedFileHost}:{ModifiedFilePath} >> {ModifiedFileNewPath}",
            fileEvent.Host,
            fileEvent.OriginalPath,
            fileEvent.Path);

        await _eventWriter.WriteAsync(
            new File(fileEvent.Host, fileEvent.Path, EventType.Renamed)
            {
                PreviousPath = fileEvent.OriginalPath,
            },
            context.CancellationToken);
    }

}