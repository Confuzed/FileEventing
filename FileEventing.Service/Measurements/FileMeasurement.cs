using System;
using FileEventing.Service.Data.Measurements;
using InfluxDB.Client.Core;

namespace FileEventing.Service.Measurements;

public abstract class FileMeasurement
{
    public FileMeasurement(string host, string path, EventType updatedReason)
        : this(host, path, updatedReason, DateTime.UtcNow)
    { }

    public FileMeasurement(string host, string path, EventType updatedReason, DateTime updated)
    {
        Host = host;
        Path = path;
        UpdatedReason = updatedReason;
        Updated = updated;
    }
    
    [Column(TagName.UpdateReceived, IsTimestamp = true)] public DateTime? Updated { get; set; }

    [Column(TagName.Host, IsTag = true)] public string Host { get; set; }

    [Column(TagName.Path, IsTag = true)] public string Path { get; set; }
    
    [Column(TagName.PreviousPath, IsTag = true)] public string? PreviousPath { get; set; }
    
    [Column(TagName.UpdatedReason, IsTag = true)] public EventType UpdatedReason { get; set; }
}