using System;
using FileEventing.Service.Data.Measurements;
using InfluxDB.Client.Core;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FileEventing.Service.Measurements;

[Measurement(MeasurementName.FileSize)]
public class FileSizeMeasurement : FileMeasurement
{
    public FileSizeMeasurement(string host, string path, long size)
        : base(host, path)
    {
        Size = size;
    }

    public FileSizeMeasurement(string host, string path, DateTime updated, long size)
        : base(host, path, updated)
    {
        Size = size;
    }

    [Column] public long Size { get; }
}