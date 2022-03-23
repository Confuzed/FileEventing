using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileEventing.Service;

public class DatabaseMaintenanceService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseMaintenanceService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var fileDataContext = scope.ServiceProvider.GetRequiredService<FileDataContext>();
        await fileDataContext.Database.EnsureCreatedAsync(stoppingToken);
    }
}