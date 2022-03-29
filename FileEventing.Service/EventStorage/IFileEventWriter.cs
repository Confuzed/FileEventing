namespace FileEventing.Service.EventStorage;

public interface IFileEventWriter
{
    Task WriteAsync();
}