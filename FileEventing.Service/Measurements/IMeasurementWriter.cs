using System.Threading;
using System.Threading.Tasks;
using FileEventing.Service.Data.Measurements;

namespace FileEventing.Service.Measurements;

public interface IMeasurementWriter
{
    Task WriteAsync(FileSizeMeasurement fileSizeMeasurement, CancellationToken cancellationToken);
}