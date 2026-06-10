using MermaYT.Core.YouTubeDownloader.Events;

namespace MermaYT.Core.YouTubeDownloader.Abstractions;

public interface IYouTubeDownloadAdapter
{
    public event EventHandler<DownloadErrorEventArgs>? DownloadErrorReceived;

    public int Download(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory);

    public void CancelDownload(
        int downloadItemId);
}
