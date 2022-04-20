using FileEventing.Contract;
using FileEventing.Contract.Events;

namespace FileEventing.Monitor.Events;

public sealed record FileRenamedEvent(string Host, string Path, string OriginalPath) : IFileRenamedEvent;