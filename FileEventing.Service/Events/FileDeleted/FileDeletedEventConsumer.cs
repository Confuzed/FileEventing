using System.Threading.Tasks;
using FileEventing.Contract.Events;
using FileEventing.Service.Measurements;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileDeleted;

public class FileDeletedEventConsumer : IConsumer<IFileDeletedEvent>
{
    private readonly ILogger<FileDeletedEventConsumer> _logger;
    private readonly IMeasurementWriter _eventWriter;

    public FileDeletedEventConsumer(
        ILogger<FileDeletedEventConsumer> logger,
        IMeasurementWriter eventWriter)
    {
        _logger = logger;
        _eventWriter = eventWriter;
    }
    
    public Task Consume(ConsumeContext<IFileDeletedEvent> context)
    {
        var fileEvent = context.Message;
        
        _logger.LogInformation("Received FileDeleted event for {ModifiedFileHost}:{ModifiedFilePath}",
            fileEvent.Host,
            fileEvent.Path);

        return Task.CompletedTask;
    }

}