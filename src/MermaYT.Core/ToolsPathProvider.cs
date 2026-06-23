using MermaYT.Core.YtDlp;

namespace MermaYT.Core;

internal static class ToolsPathProvider
{
    private const string ToolsDirectoryName = "Tools";

    public static string GetYtDlpFileName()
    {
        var ytDlpFileName = Path.Combine(
            AppContext.BaseDirectory,
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
            AppContext.BaseDirectory,
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
