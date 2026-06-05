using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MermaYT.Core;

public sealed class DownloadItem : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public required string Title { get; init; }

    public required string Url { get; init; }

    public required OutputFormat OutputFormat { get; init; }

    public required string? ImageUrl { get; init; }

    private DownloadState _downloadState = DownloadState.Queued;

    public DownloadState DownloadState
    {
        get => _downloadState;
        set { _downloadState = value; Notify(); }
    }

    private long _totalBytes = -1;
    public long TotalBytes
    {
        get => _totalBytes;
        set { _totalBytes = value; Notify(); Notify(nameof(Progress)); }
    }

    private long _downloadedBytes;

    public long DownloadedBytes
    {
        get => _downloadedBytes;
        set { _downloadedBytes = value; Notify(); Notify(nameof(Progress)); }
    }

    public float Progress => TotalBytes > 0 ? (float)DownloadedBytes / TotalBytes : 0;

    private string? _errorMessage;

    public string? ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; Notify(); }
    }

    private void Notify([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
