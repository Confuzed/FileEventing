using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileUpsertRequest;

public class UpsertFileRecordConsumer : IConsumer<IUpsertFileRequest>
{
    private readonly ILogger<UpsertFileRecordConsumer> _logger;
    private static int _nextId = 1;

    public UpsertFileRecordConsumer(ILogger<UpsertFileRecordConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IUpsertFileRequest> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("Upserting file record for {FileHost}:{FilePath}",
            message.Host, message.Path);

        if (context.IsResponseAccepted<IUpsertFileResult>())
        {
            await context.RespondAsync<IUpsertFileResult>(new UpsertFileResult(true, _nextId++));
        }
    }
}