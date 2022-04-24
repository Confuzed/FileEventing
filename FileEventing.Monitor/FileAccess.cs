namespace FileEventing.Monitor;

public class FileAccess : IFileAccess
{
    public long GetLength(string path)
        => new FileInfo(path).Length;
}