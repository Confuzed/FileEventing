using System.Threading;
using System.Threading.Tasks;
using FileEventing.Service.Data.Measurements;
using FileEventing.Service.Measurements;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;

namespace FileEventing.Service.Data.InfluxDb;

public class InfluxDbMeasurementWriter : IMeasurementWriter
{
    private readonly IOptionsMonitor<InfluxDbOptions> _optionsMonitor;

    public InfluxDbMeasurementWriter(IOptionsMonitor<InfluxDbOptions> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }
    
    public async Task WriteAsync(FileSizeMeasurement fileSizeMeasurement, CancellationToken cancellationToken)
    {
        var options = _optionsMonitor.CurrentValue;

        var influxConfig = InfluxDBClientOptions.Builder.CreateNew()
            .Org(options.Organisation)
            .Bucket(options.Bucket)
            .Url(options.ServiceUri)
            .AuthenticateToken(options.Token)
            .Build();
        
        using var influxDb = InfluxDBClientFactory.Create(influxConfig);
        
        var writer = influxDb.GetWriteApiAsync();
        
        await writer.WriteMeasurementAsync(
            fileSizeMeasurement,
            precision: WritePrecision.Ns,
            bucket: options.Bucket,
            cancellationToken: cancellationToken);
    }
}