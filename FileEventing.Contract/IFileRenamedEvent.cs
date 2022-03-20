namespace FileEventing.Contract;

public interface IFileRenamedEvent : IFileEvent
{
    string OriginalPath { get; }
}