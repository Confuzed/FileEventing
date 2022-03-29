using System.ComponentModel.DataAnnotations;

namespace FileEventing.Service.Configuration;

public class InfluxDbOptions
{
    public const string SectionName = "InfluxDb";
    
    [Required, Url]
    public string ServiceUri { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;
}