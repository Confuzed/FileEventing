namespace FileEventing.Shared.Configuration;

public class BusOptions
{
    public const string ConfigurationSectionName = "Bus";

    public string Host { get; set; } = null!;

    public string User { get; set; } = null!;

    public string Password { get; set; } = null!;
}