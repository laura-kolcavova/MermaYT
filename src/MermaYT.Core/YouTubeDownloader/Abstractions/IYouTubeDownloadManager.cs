using MermaYT.Core.YouTubeDownloader.Events;

namespace MermaYT.Core.YouTubeDownloader.Abstractions;

public interface IYouTubeDownloadManager
{
    public event EventHandler<ProgressUpdatedEventArgs>? ProgressUpdated;

    public event EventHandler<ConvertingStartedEventArgs>? ConvertingStarted;

    public event EventHandler<ErrorOccurredEventArgs>? ErrorOccurred;

    public event EventHandler<FailedEventArgs>? Failed;

    public event EventHandler<CompletedEventArgs>? Completed;

    public int Download(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory);

    public void Cancel(
        int downloadItemId);
}
