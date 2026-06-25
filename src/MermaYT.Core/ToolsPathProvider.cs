using MermaYT.Core.YtDlp;

namespace MermaYT.Core;

internal static class ToolsPathProvider
{
    private const string ToolsDirectoryName = "Tools";

    private static string BaseDirectory =>
        Path.GetDirectoryName(Environment.ProcessPath)
        ?? AppContext.BaseDirectory;

    public static string GetYtDlpFileName()
    {
        var ytDlpFileName = Path.Combine(
            BaseDirectory,
            ToolsDirectoryName,
            YtDlpConstants.YtDlpExecutableName);

        if (!File.Exists(ytDlpFileName))
        {
            throw new FileNotFoundException(
                "yt-dlp executable not found.",
                ytDlpFileName);
        }

        return ytDlpFileName;
    }

    public static string GetFfmpegFileName()
    {
        var ffMpegFileName = Path.Combine(
            BaseDirectory,
            ToolsDirectoryName,
            YtDlpConstants.FfmpegExecutableName);

        if (!File.Exists(ffMpegFileName))
        {
            throw new FileNotFoundException(
                "ffmpeg executable not found.",
                ffMpegFileName);
        }

        return ffMpegFileName;
    }
}
