using System;
using FileEventing.Service.Data.Measurements;
using InfluxDB.Client.Core;

namespace FileEventing.Service.Measurements;

public abstract class FileMeasurement
{
    public FileMeasurement(string host, string path)
        : this(host, path, DateTime.UtcNow)
    { }

    public FileMeasurement(string host, string path, DateTime updated)
    {
        Host = host;
        Path = path;
        Updated = updated;
    }
    
    [Column(TagName.UpdateReceived, IsTimestamp = true)] public DateTime? Updated { get; set; }

    [Column(TagName.Host, IsTag = true)] public string Host { get; set; }

    [Column(TagName.Path, IsTag = true)] public string Path { get; set; }
}