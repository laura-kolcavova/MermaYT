namespace MermaYT.Core.YouTubeDownloader.Events;

public sealed record ProgressReceivedEventArgs
{
    public required int DownloadItemId { get; init; }

    public required float ProgressPercentage { get; init; }

    public required long DownloadedBytes { get; init; }

    public required long TotalBytes { get; init; }

    public required string Title { get; init; }
}
