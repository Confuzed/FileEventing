using FileEventing.Contract;
using FileEventing.Contract.Events;

namespace FileEventing.Monitor.Events;

public record FileCreatedEvent(string Host, string Path) : IFileCreatedEvent;