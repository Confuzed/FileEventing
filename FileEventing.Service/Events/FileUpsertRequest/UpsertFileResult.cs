namespace FileEventing.Service.Events.FileUpsertRequest;

public interface IUpsertFileResult
{
    bool Succeeded { get; }
    
    int FileId { get; }
}

public sealed record UpsertFileResult(bool Succeeded, int FileId) : IUpsertFileResult;