using FileEventing.Contract;

namespace FileEventing.Monitor.Events;

public record FileCreatedEvent(string Host, string Path) : IFileCreatedEvent;