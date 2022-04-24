using FileEventing.Contract.Events;

namespace FileEventing.Monitor.Events;

public sealed record FileChangedEvent(string Host, string Path, long Length) : IFileChangedEvent;