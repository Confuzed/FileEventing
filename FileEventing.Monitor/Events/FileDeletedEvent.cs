using FileEventing.Contract;
using FileEventing.Contract.Events;

namespace FileEventing.Monitor.Events;

public sealed record FileDeletedEvent(string Host, string Path) : IFileDeletedEvent;