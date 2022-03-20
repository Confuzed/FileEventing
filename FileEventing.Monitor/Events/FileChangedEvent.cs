using FileEventing.Contract;

namespace FileEventing.Monitor.Events;

public sealed record FileChangedEvent(string Host, string Path) : IFileChangedEvent;