using System.Net;
using FileEventing.Monitor.Events;
using MassTransit;

namespace FileEventing.Monitor;

public static class FileEventPublishingExtensions
{
    public static async Task PublishFileChanged(this IBus bus, string path, long fileSizeInBytes)
    {
        await bus.PublishFileSize(path, fileSizeInBytes);
        await bus.Publish(new FileChangedEvent(GetHostName(), path));
    }

    public static async Task PublishFileCreated(this IBus bus, string path, long fileSizeInBytes)
    {
        await bus.PublishFileSize(path, fileSizeInBytes);
        await bus.Publish(new FileCreatedEvent(GetHostName(), path));
    }

    public static async Task PublishFileDeleted(this IBus bus, string path)
    {
        await bus.PublishFileSize(path, 0);
        await bus.Publish(new FileDeletedEvent(GetHostName(), path));
    }

    public static Task PublishFileRenamed(this IBus bus, string originalPath, string newPath) =>
        bus.Publish(new FileRenamedEvent(GetHostName(), originalPath, newPath));
    
    private static Task PublishFileSize(this IBus bus, string path, long fileSizeInBytes) =>
        bus.Publish(new FileSizeEvent(GetHostName(), path, fileSizeInBytes));

    private static string GetHostName() => LazyHostName.Value;

    private static readonly Lazy<string> LazyHostName = new(Dns.GetHostName);
}