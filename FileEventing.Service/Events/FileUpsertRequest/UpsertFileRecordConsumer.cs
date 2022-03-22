using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FileEventing.Service.Events.FileUpsertRequest;

public class UpsertFileRecordConsumer : IConsumer<IUpsertFileRequest>
{
    private readonly FileDataContext _fileDataContext;
    private readonly ILogger<UpsertFileRecordConsumer> _logger;

    public UpsertFileRecordConsumer(
        FileDataContext fileDataContext,
        ILogger<UpsertFileRecordConsumer> logger)
    {
        _fileDataContext = fileDataContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IUpsertFileRequest> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("Upserting file record for {FileHost}:{FilePath}",
            message.Host, message.Path);

        var file = await LookupFile(message.Host, message.Path);

        if (file == null)
        {
            try
            {
                var entry = await _fileDataContext.Files.AddAsync(new Model.File
                {
                    Host = message.Host,
                    Path = message.Path,
                });

                await _fileDataContext.SaveChangesAsync();

                file = entry.Entity;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed adding file to db");

                file = await LookupFile(message.Host, message.Path);
            }

            if (file == null)
                throw new InvalidOperationException("Unable to upsert file");
        }

        if (context.IsResponseAccepted<IUpsertFileResult>())
        {
            await context.RespondAsync<IUpsertFileResult>(new UpsertFileResult(true, file.Id));
        }
    }
    
    private Task<Model.File?> LookupFile(string host, string path) =>
        _fileDataContext.Files.SingleOrDefaultAsync(f =>
            f.Host == host && f.Path == path);
}