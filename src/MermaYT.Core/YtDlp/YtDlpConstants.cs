namespace MermaYT.Core.YtDlp;

internal static class YtDlpConstants
{
    public const string YtDlpExecutableName = "yt-dlp.exe";

    public const string FFmpegExecutableName = "ffmpeg.exe";

    public const string ErrorPrefix = "ERROR: ";

    public const string ConvertingPrefix = "[ExtractAudio] Destination: ";

    public const string ProgressStatusDownloading = "downloading";

    public const string ProgressStatusFinished = "finished";

    public const string ProgressPrefix = "[progress]";

    public const string ProgressSeparator = "__";

    public const string ProgressTemplate = "[progress]__%(progress.status)s__%(progress._percent)s__%(progress.downloaded_bytes)s__%(progress.total_bytes)s__%(info.title)s";
}
