namespace FileEventing.Service.Events.FileUpsertRequest;

public sealed record UpsertFileRequest(string Host, string Path, string? NewPath) : IUpsertFileRequest;