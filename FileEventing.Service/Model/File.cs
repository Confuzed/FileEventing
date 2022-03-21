using System.ComponentModel.DataAnnotations;

namespace FileEventing.Service.Model;

public class File
{
    [Key]
    public int Id { get; set; }

    public string Host { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;
}