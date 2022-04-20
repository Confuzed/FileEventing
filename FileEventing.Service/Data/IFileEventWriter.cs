using System.Threading;
using System.Threading.Tasks;
using FileEventing.Service.Data.Model;

namespace FileEventing.Service.Data;

public interface IFileEventWriter
{
    Task WriteAsync(File file, CancellationToken cancellationToken);
}