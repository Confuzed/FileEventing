namespace FileEventing.Contract;

public interface IFileEvent
{
    string Host { get; }
    string Path { get; }
}