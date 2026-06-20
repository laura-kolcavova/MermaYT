namespace MermaYT.Core.YouTubeDownloader.Events;

public sealed class FailedEventArgs : EventArgs
{
    public required int DownloadItemId { get; init; }

    public required string ErrorMessage { get; init; }
}
