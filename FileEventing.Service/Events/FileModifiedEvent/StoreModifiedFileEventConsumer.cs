﻿using FileEventing.Contract;
using FileEventing.Service.Events.FileUpsertRequest;
using FileEventing.Service.Extensions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileModifiedEvent;

public class StoreModifiedFileEventConsumer<TFileEvent> : IConsumer<TFileEvent>
    where TFileEvent : class, IFileEvent
{
    private readonly ILogger<StoreModifiedFileEventConsumer<TFileEvent>> _logger;
    private readonly IRequestClient<IUpsertFileRequest> _upsertFileRequestClient;

    public StoreModifiedFileEventConsumer(
        ILogger<StoreModifiedFileEventConsumer<TFileEvent>> logger,
        IRequestClient<IUpsertFileRequest> upsertFileRequestClient)
    {
        _logger = logger;
        _upsertFileRequestClient = upsertFileRequestClient;
    }
    
    public async Task Consume(ConsumeContext<TFileEvent> context)
    {
        var fileEvent = context.Message;
        
        _logger.LogInformation("Received {FileModificationType} event for {ModifiedFileHost}:{ModifiedFilePath}",
            fileEvent.GetModificationTypeName(),
            fileEvent.Host,
            fileEvent.Path);

        var fileId = await InsertOrUpdateFileReturningId(fileEvent);

        await InsertFileEvent(fileEvent);
    }

    private async Task<int> InsertOrUpdateFileReturningId(TFileEvent fileEvent)
    {
        var upsertResult = await _upsertFileRequestClient.GetResponse<IUpsertFileResult>(
            new UpsertFileRequest(fileEvent.Host, fileEvent.Path));

        if (!upsertResult.Message.Succeeded)
        {
            _logger.LogWarning("Upsert request failed");
            throw new Exception("Failed to obtain a file record");
        }

        _logger.LogInformation("Upsert request succeeded, file has Id {FileId}",
            upsertResult.Message.FileId);

        return upsertResult.Message.FileId;
    }

    private async Task InsertFileEvent(TFileEvent fileEvent)
    {
        _logger.LogInformation("Commiting file modification data");
        await Task.Delay(250);
    }
}