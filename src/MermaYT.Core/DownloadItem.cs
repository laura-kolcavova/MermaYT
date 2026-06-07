using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MermaYT.Core;

public sealed class DownloadItem : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public OutputFormat OutputFormat { get; set; } = OutputFormat.MP3;

    public string ImageUrl { get; set; } = string.Empty;

    public string DestinationFolder { get; set; } = string.Empty;

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

    private long _downloadedBytes = -1;

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
