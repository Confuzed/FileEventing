using Microsoft.Extensions.Hosting;

namespace FileEventing.Service;

public class DatabaseMaintenanceService : BackgroundService
{
    private readonly FileDataContext _fileData;

    public DatabaseMaintenanceService(FileDataContext fileData)
    {
        _fileData = fileData;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _fileData.Database.EnsureCreatedAsync(stoppingToken);
    }
}