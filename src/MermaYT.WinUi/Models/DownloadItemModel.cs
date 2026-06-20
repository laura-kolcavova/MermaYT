using MermaYT.Core.YouTubeDownloader;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MermaYT.WinUi.Models;

public sealed class DownloadItemModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int ProcessId { get; set; } = -1;

    public string Url { get; set; } = string.Empty;

    public OutputFormat OutputFormat { get; set; } = OutputFormat.MP3;

    public string DestinationFolder { get; set; } = string.Empty;

    private DownloadState _downloadState = DownloadState.Processing;

    public DownloadState DownloadState
    {
        get => _downloadState;
        set
        {
            _downloadState = value;
            NotifyPropertyChanged();
        }
    }

    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            NotifyPropertyChanged();
        }
    }

    private string _imageUrl = string.Empty;

    public string ImageUrl
    {
        get => _imageUrl;
        set
        {
            _imageUrl = value;
            NotifyPropertyChanged();
        }
    }

    private float _progressPercentage = 0;

    public float ProgressPercentage
    {
        get => _progressPercentage;
        set
        {
            _progressPercentage = value;
            NotifyPropertyChanged();
        }
    }

    private long _downloadedBytes = 0;

    public long DownloadedBytes
    {
        get => _downloadedBytes;
        set
        {
            _downloadedBytes = value;
            NotifyPropertyChanged();
        }
    }

    private long _totalBytes = 0;

    public long TotalBytes
    {
        get => _totalBytes;
        set
        {
            _totalBytes = value;
            NotifyPropertyChanged();
        }
    }

    private string? _errorMessage;

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            NotifyPropertyChanged();
        }
    }

    public void UpdateDownloadingState(
        float progressPercentage,
        long downloadedBytes,
        long totalBytes,
        string title)
    {
        if (DownloadState == DownloadState.Converting ||
            DownloadState == DownloadState.Error ||
            DownloadState == DownloadState.Completed)
        {
            return;
        }

        DownloadState = DownloadState.Downloading;
        ProgressPercentage = progressPercentage;
        DownloadedBytes = downloadedBytes;
        TotalBytes = totalBytes;
        Title = title;
    }

    public void UpdateConvertingState()
    {
        if (DownloadState == DownloadState.Converting ||
            DownloadState == DownloadState.Error ||
            DownloadState == DownloadState.Completed)
        {
            return;
        }

        DownloadState = DownloadState.Converting;
        ProgressPercentage = 100;
    }

    public void UpdateErrorState(
        string errorMessage)
    {
        if (DownloadState == DownloadState.Completed)
        {
            return;
        }

        DownloadState = DownloadState.Error;
        ErrorMessage = errorMessage;
    }

    public void UpdateCompletedState()
    {
        if (DownloadState == DownloadState.Error)
        {
            return;
        }

        DownloadState = DownloadState.Completed;
        ProgressPercentage = 100;
    }

    private void NotifyPropertyChanged(
        [CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(name));
    }
}
