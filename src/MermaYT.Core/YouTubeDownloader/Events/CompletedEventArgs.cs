namespace MermaYT.Core.YouTubeDownloader.Events;

internal sealed record CompletedEventArgs
{
    public required int DownloadItemId { get; init; }
}
