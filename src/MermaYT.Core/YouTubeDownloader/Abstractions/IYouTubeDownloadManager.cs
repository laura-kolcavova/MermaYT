namespace MermaYT.Core.YouTubeDownloader.Abstractions;

public interface IYouTubeDownloadManager
{
    public Task DownloadAsync(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory,
        CancellationToken cancellationToken = default);
}
