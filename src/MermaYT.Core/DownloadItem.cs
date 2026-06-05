namespace MermaYT.Core;

public sealed class DownloadItem
{
    public required string Title { get; set; }

    public required string Url { get; set; }

    public required OutputFormat OutputFormat { get; set; }

    public DownloadState DownloadState { get; set; } = DownloadState.Queued;

    public long TotalBytes { get; set; } = -1;

    public long DownloadedBytes { get; set; }

    public float Progress { get; set; }

    public required string? ImageUrl { get; set; }

    public string? ErrorMessage { get; set; }

    public float GetProgress() =>
        TotalBytes > 0
            ? (float)DownloadedBytes / TotalBytes
            : 0;
}
