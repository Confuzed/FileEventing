namespace FileEventing.Contract.Events;

public interface IFileEvent
{
    string Host { get; }
    string Path { get; }
}