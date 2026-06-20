namespace MermaYT.Core.YouTubeDownloader.Events;

public sealed class ConvertingStartedEventArgs
{
    public required int DownloadItemId { get; init; }
}
