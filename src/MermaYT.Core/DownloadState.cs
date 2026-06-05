namespace MermaYT.Core;

public enum DownloadState
{
    Queued,

    Processing,

    Downloading,

    Converting,

    Completed,

    Failed,

    Paused,

    Cancelled,
}
