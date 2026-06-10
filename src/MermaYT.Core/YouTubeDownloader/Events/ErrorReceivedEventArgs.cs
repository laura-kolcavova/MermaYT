namespace MermaYT.Core.YouTubeDownloader.Events;

public sealed class ErrorReceivedEventArgs : EventArgs
{
    public required int DownloadItemId { get; init; }

    public required string ErrorMessage { get; init; }
}
