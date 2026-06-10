using MermaYT.Core.YouTubeDownloader.Events;

namespace MermaYT.Core.YouTubeDownloader.Abstractions;

public interface IYouTubeDownloadManager
{
    public event EventHandler<ProgressReceivedEventArgs>? ProgressReceived;

    public event EventHandler<ErrorReceivedEventArgs>? ErrorReceived;

    public int Download(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory);

    public void Cancel(
        int downloadItemId);
}
