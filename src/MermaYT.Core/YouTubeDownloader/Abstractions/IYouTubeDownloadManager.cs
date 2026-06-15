using MermaYT.Core.YouTubeDownloader.Events;

namespace MermaYT.Core.YouTubeDownloader.Abstractions;

public interface IYouTubeDownloadManager
{
    public event EventHandler<ProgressUpdatedEventArgs>? ProgressUpdated;

    public event EventHandler<ErrorReceivedEventArgs>? ErrorReceived;

    public event EventHandler<CompletedEventArgs>? Completed;

    public int Download(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory);

    public void Cancel(
        int downloadItemId);
}
