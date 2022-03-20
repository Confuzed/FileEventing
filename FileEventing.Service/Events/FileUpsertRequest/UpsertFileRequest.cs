using FileEventing.Contract;

namespace FileEventing.Service.Events.FileUpsertRequest;

public interface IUpsertFileRequest : IFileEvent {}

public sealed record UpsertFileRequest(string Host, string Path) : IUpsertFileRequest;