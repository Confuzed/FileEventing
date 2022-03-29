using FileEventing.Service.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using File = FileEventing.Service.Data.Entities.File;

namespace FileEventing.Service.Events.FileUpsertRequest;

public class UpsertFileRecordConsumer
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


}