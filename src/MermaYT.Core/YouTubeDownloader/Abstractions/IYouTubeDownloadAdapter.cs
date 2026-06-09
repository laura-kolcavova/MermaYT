namespace MermaYT.Core.YouTubeDownloader.Abstractions;

public interface IYouTubeDownloadAdapter
{
    public int Download(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory);

    public void CancelDownload(
        int downloadItemId);
}
