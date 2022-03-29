using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FileEventing.Service.Data.Entities;
using FileEventing.Service.Events.FileUpsertRequest;
using Microsoft.EntityFrameworkCore;

namespace FileEventing.Service.Data;

//using File = FileEventing.Service.Data.Entities.File;

public interface IFileDataRepository
{
    Task<File> Upsert(File fileData);
}

public class EfFileDataRepository : IFileDataRepository
{
    private readonly FileDataContext _fileDataContext;
    private readonly ILogger<EfFileDataRepository> _logger;

    public EfFileDataRepository(FileDataContext fileDataContext, ILogger<EfFileDataRepository> logger)
    {
        _fileDataContext = fileDataContext;
        _logger = logger;
    }

    public Task<File> Get(string host, string path) =>
        _fileDataContext.Files.SingleAsync(file => file.Host.Equals(host) && file.Path.Equals(path));

    public Task<File> Get(string reference) => Get(EntityReference.Decode(reference));

    public async Task<File> Get(int id)
    
    public async Task<File> Upsert(File fileData)
    {
        var host = fileData.Host;
        var path = fileData.Path;

        _logger.LogDebug("Upserting file record for {FileHost}:{FilePath}",
            host, path);

        var file = await LookupFile(host, path);

        if (file == null)
        {
            try
            {
                var entry = await _fileDataContext.Files.AddAsync(fileData);
                await _fileDataContext.SaveChangesAsync();
                file = entry.Entity;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed adding file to db");

                file = await LookupFile(host, path);
            }

            if (file == null)
                throw new InvalidOperationException("Unable to upsert file");
        }
        else if (message.NewPath != null)
        {
            file.Path = message.NewPath;
            _fileDataContext.Files.Update(file);
            await _fileDataContext.SaveChangesAsync();
        }

        if (context.IsResponseAccepted<IUpsertFileResult>())
        {
            await context.RespondAsync<IUpsertFileResult>(new UpsertFileResult(true, file.Id));
        }
    }
    
    private Task<File?> LookupFile(string host, string path) =>
        _fileDataContext.Files.SingleOrDefaultAsync(f =>
            f.Host == host && f.Path == path);
}