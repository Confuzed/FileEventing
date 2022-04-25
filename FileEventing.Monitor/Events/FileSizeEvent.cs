using FileEventing.Contract.Events;

namespace FileEventing.Monitor.Events;

public sealed record FileSizeEvent(string Host, string Path, long Length) : IFileSizeEvent;