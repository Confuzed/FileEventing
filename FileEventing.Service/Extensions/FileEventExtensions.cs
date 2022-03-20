using FileEventing.Contract;

namespace FileEventing.Service.Extensions;

internal static class FileEventExtensions
{
    /// <summary>
    /// Get the name of the modification event represented by <paramref name="fileEvent"/>.
    /// </summary>
    /// <param name="fileEvent">The source file event.</param>
    /// <returns>The modification event name.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="fileEvent"/> is not a type representing a file modification event
    /// handled by this method.
    /// </exception>
    internal static string GetModificationTypeName(this IFileEvent fileEvent) =>
        fileEvent switch
        {
            IFileChangedEvent => FileModificationTypeName.FileChanged,
            IFileCreatedEvent => FileModificationTypeName.FileCreated,
            IFileDeletedEvent => FileModificationTypeName.FileDeleted,
            IFileRenamedEvent => FileModificationTypeName.FileRenamed,
            _ => throw new ArgumentOutOfRangeException(nameof(fileEvent), "fileEvent is not a recognised value")
        };
}