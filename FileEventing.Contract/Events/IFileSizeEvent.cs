namespace FileEventing.Contract.Events;

public interface IFileSizeEvent : IFileEvent
{
    long Length { get; }
}