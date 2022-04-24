using System.ComponentModel.DataAnnotations;

namespace FileEventing.Service.Data.InfluxDb;

public class InfluxDbOptions
{
    public const string SectionName = "InfluxDb";
    
    [Required, Url]
    public string ServiceUri { get; set; } = string.Empty;

    [Required]
    public string Organisation { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string Bucket { get; set; } = string.Empty;
}