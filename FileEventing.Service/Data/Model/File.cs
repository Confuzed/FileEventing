using System;
using InfluxDB.Client.Core;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FileEventing.Service.Data.Model;

public class File
{
    public File(string host, string path, EventType updatedReason)
        : this(host, path, updatedReason, DateTime.UtcNow)
    { }

    public File(string host, string path, EventType updatedReason, DateTime updated)
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

    [Column(FieldName.FileSize, IsMeasurement = true)] public long Size { get; set; } = 0;
}