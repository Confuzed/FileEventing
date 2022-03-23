using FileEventing.Contract;

namespace FileEventing.Service.Events.FileUpsertRequest;

public interface IUpsertFileRequest : IFileEvent
{
    string? NewPath { get; }
}

public sealed record UpsertFileRequest(string Host, string Path, string? NewPath) : IUpsertFileRequest;