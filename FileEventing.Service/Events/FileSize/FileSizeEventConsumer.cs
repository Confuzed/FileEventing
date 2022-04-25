using System.Threading.Tasks;
using FileEventing.Contract.Events;
using FileEventing.Service.Measurements;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileSize;

public class FileSizeEventConsumer : IConsumer<IFileSizeEvent>
{
    private readonly ILogger<FileSizeEventConsumer> _logger;
    private readonly IMeasurementWriter _measurementWriter;

    public FileSizeEventConsumer(
        ILogger<FileSizeEventConsumer> logger,
        IMeasurementWriter measurementWriter)
    {
        _logger = logger;
        _measurementWriter = measurementWriter;
    }

    public async Task Consume(ConsumeContext<IFileSizeEvent> context)
    {
        var fileEvent = context.Message;
        
        _logger.LogDebug(
            "Received FileSize event; {ModifiedFileHost}:{ModifiedFilePath} = {FileLengthInBytes}",
            fileEvent.Host,
            fileEvent.Path,
            fileEvent.Length);
        
        await _measurementWriter.WriteAsync(
            new FileSizeMeasurement(fileEvent.Host, fileEvent.Path, fileEvent.Length),
            context.CancellationToken);
    }
}