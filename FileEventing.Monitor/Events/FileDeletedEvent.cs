using FileEventing.Contract;

namespace FileEventing.Monitor.Events;

public sealed record FileDeletedEvent(string Host, string Path) : IFileDeletedEvent;