using System;
using FileEventing.Service.Data.Measurements;
using InfluxDB.Client.Core;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FileEventing.Service.Measurements;

[Measurement(MeasurementName.FileSize)]
public class FileSizeMeasurement : FileMeasurement
{
    public FileSizeMeasurement(string host, string path, EventType updatedReason, long size)
        : this(host, path, updatedReason, DateTime.UtcNow, size)
    { }

    public FileSizeMeasurement(string host, string path, EventType updatedReason, DateTime updated, long size)
        : base(host, path, updatedReason, updated)
    {
        Size = size;
    }
    [Column(FieldName.FileSize)] public long Size { get; set; } = 0;
}