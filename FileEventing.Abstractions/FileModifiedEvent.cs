namespace FileEventing.Abstractions;

public sealed record FileModifiedEvent(string Path)
{
}