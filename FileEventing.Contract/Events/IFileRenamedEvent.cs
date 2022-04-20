namespace FileEventing.Contract.Events;

public interface IFileRenamedEvent : IFileEvent
{
    string OriginalPath { get; }
}