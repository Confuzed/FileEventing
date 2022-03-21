using Microsoft.EntityFrameworkCore;

namespace FileEventing.Service;

public class FileDataContext : DbContext
{
    public FileDataContext(DbContextOptions<FileDataContext> options)
        : base(options)
    {}

    public DbSet<Model.File> Files { get; set; } = null!;
}