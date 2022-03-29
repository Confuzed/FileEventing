using FileEventing.Service.Configuration;
using InfluxDB.Client;
using Microsoft.Extensions.Options;

namespace FileEventing.Service.EventStorage;

public class InfluxDbFileEventWriter : IFileEventWriter
{
    private readonly IOptionsMonitor<InfluxDbOptions> _optionsMonitor;

    public InfluxDbFileEventWriter(IOptionsMonitor<InfluxDbOptions> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }
    
    public async Task WriteAsync()
    {
        var options = _optionsMonitor.CurrentValue;

        using var influxDb = InfluxDBClientFactory.Create(options.ServiceUri, options.Token);
        var writer = influxDb.GetWriteApiAsync();
        writer.WriteMeasurementAsync()
        
        throw new NotImplementedException();
    }
}