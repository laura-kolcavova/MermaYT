using MermaYT.Core.YouTubeDownloader;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MermaYT.WinUi.Models;

public sealed class DownloadItemModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int ProcessId { get; set; } = -1;

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public OutputFormat OutputFormat { get; set; } = OutputFormat.MP3;

    public string ImageUrl { get; set; } = string.Empty;

    public string DestinationFolder { get; set; } = string.Empty;

    private DownloadState _downloadState = DownloadState.Queued;

    public DownloadState DownloadState
    {
        get => _downloadState;
        set
        {
            _downloadState = value;
            NotifyPropertyChanged();
        }
    }

    private long _totalBytes = -1;

    public long TotalBytes
    {
        get => _totalBytes;
        set
        {
            _totalBytes = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Progress));
        }
    }

    private long _downloadedBytes = -1;

    public long DownloadedBytes
    {
        get => _downloadedBytes;
        set
        {
            _downloadedBytes = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Progress));
        }
    }

    public float Progress => TotalBytes > 0 ? (float)DownloadedBytes / TotalBytes : 0;

    private string? _errorMessage;

    public string? ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; NotifyPropertyChanged(); }
    }

    private void NotifyPropertyChanged(
        [CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
