using FileEventing.Contract;

namespace FileEventing.Service.Events.FileUpsertRequest;

public interface IUpsertFileRequest : IFileEvent
{
    string? NewPath { get; }
}