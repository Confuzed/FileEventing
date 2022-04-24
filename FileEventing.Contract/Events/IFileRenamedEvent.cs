namespace FileEventing.Contract.Events;

public interface IFileRenamedEvent : IFileSizeEvent
{
    string OriginalPath { get; }
}