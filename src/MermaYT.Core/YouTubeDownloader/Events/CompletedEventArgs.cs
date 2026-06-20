namespace MermaYT.Core.YouTubeDownloader.Events;

public sealed record CompletedEventArgs
{
    public required int DownloadItemId { get; init; }
}
