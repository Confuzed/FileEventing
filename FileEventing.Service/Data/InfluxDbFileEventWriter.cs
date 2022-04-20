using System.Threading;
using System.Threading.Tasks;
using FileEventing.Service.Configuration;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;
using File = FileEventing.Service.Data.Model.File;

namespace FileEventing.Service.Data;

public class InfluxDbFileEventWriter : IFileEventWriter
{
    private readonly IOptionsMonitor<InfluxDbOptions> _optionsMonitor;

    public InfluxDbFileEventWriter(IOptionsMonitor<InfluxDbOptions> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }
    
    public async Task WriteAsync(File file, CancellationToken cancellationToken)
    {
        var options = _optionsMonitor.CurrentValue;

        using var influxDb = InfluxDBClientFactory.Create(options.ServiceUri, options.Token);
        var writer = influxDb.GetWriteApiAsync();
        
        await writer.WriteMeasurementAsync(
            file,
            precision: WritePrecision.Ms,
            bucket: options.Bucket,
            cancellationToken: cancellationToken);
    }
}