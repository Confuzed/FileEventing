using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FileEventing.Service.Model;

[Index(nameof(File.Host), nameof(File.Path), IsUnique = true)]
public class File
{
    [Key]
    public int Id { get; set; }

    public string Host { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;
}