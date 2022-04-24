namespace FileEventing.Monitor;

/// <summary>
/// A service for anything requiring direct file access.
/// </summary>
public interface IFileAccess
{
    long GetLength(string path);
}