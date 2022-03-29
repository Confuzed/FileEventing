using Microsoft.EntityFrameworkCore;
using File = FileEventing.Service.Data.Entities.File;

namespace FileEventing.Service.Data;

public class FileDataContext : DbContext
{
    public FileDataContext(DbContextOptions<FileDataContext> options)
        : base(options)
    {}

    public DbSet<File> Files { get; set; } = null!;
}