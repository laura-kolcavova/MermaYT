using MermaYT.Core.YouTubeDownloader;
using MermaYT.Core.YouTubeDownloader.Abstractions;
using MermaYT.Core.YouTubeDownloader.Events;
using System.Diagnostics;

namespace MermaYT.Core.YtDlp;

internal sealed class YtDlpDownloadAdapter
    : IYouTubeDownloadManager
{
    public event EventHandler<ProgressReceivedEventArgs>? ProgressReceived;

    public event EventHandler<ErrorReceivedEventArgs>? ErrorReceived;

    private readonly Dictionary<int, Process> _downloadProcessesByProcessId = [];

    public void Cancel(
        int downloadItemId)
    {
        if (!_downloadProcessesByProcessId.TryGetValue(
            downloadItemId,
            out var process))
        {
            return;
        }

        if (!process.HasExited)
        {
            return;
        }

        process.Kill();
    }

    public int Download(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory)
    {
        if (string.IsNullOrEmpty(youTubeUrl))
        {
            throw new InvalidOperationException("URL must be provided.");
        }

        if (string.IsNullOrEmpty(outputDirectory))
        {
            throw new InvalidOperationException("Output directory must be provided.");
        }

        var ytDlpFileName = GetYtDlpFileName();

        var ffMpegFileName = GetFFmpegFileName();

        var argumentsBuilder = YtDlpArgumentsBuilder
            .New()
            .NewLine()
            .NoSimulate()
            .NoOverwrites()
            .NoPlaylist()
            .EmbedThumbnail()
            .AddMetaData()
            .Progress()
            .ProgressTemplate(YtDlpConstants.ProgressTemplate)
            .OutputTemplate(outputFormat)
            .OutputDirectory(outputDirectory)
            .FFmpegLocation(ffMpegFileName)
            .Url(youTubeUrl);

        var arguments = argumentsBuilder.Build();

        Debug.WriteLine($"[process]: {ytDlpFileName} {arguments}");

        var startInfo = new ProcessStartInfo
        {
            FileName = ytDlpFileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        var process = new Process()
        {
            StartInfo = startInfo
        };

        process.EnableRaisingEvents = true;

        process.OutputDataReceived += OnOutputDataReceived;

        process.ErrorDataReceived += OnErrorDataReceived;

        process.Exited += OnExited;

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        _downloadProcessesByProcessId.Add(process.Id, process);

        return process.Id;
    }

    private void OnOutputDataReceived(
       object sender,
       DataReceivedEventArgs e)
    {
        if (sender is not Process process ||
            string.IsNullOrEmpty(e.Data))
        {
            return;
        }

        Debug.WriteLine(e.Data);

        if (e.Data.StartsWith(
            YtDlpConstants.ProgressPrefix,
            StringComparison.OrdinalIgnoreCase))
        {
            var progressData = e.Data.Split(
                YtDlpConstants.ProgressSeparator,
                StringSplitOptions.RemoveEmptyEntries);

            var status = progressData[1];
            var percent = progressData[2];
            var downloadedBytes = progressData[3];
            var totalBytes = progressData[4];
            var title = progressData[5];

            _ = float.TryParse(percent, out var parsedPercent);
            _ = long.TryParse(downloadedBytes, out var parsedDownloadedBytes);
            _ = long.TryParse(totalBytes, out var parsedTotalBytes);

            var eventArgs = new ProgressReceivedEventArgs
            {
                DownloadItemId = process.Id,
                ProgressPercentage = parsedPercent,
                DownloadedBytes = parsedDownloadedBytes,
                TotalBytes = parsedTotalBytes,
                Title = title
            };

            ProgressReceived?.Invoke(this, eventArgs);
        }
    }

    private void OnErrorDataReceived(
        object sender,
        DataReceivedEventArgs e)
    {
        if (sender is not Process process ||
            string.IsNullOrEmpty(e.Data))
        {
            return;
        }

        Debug.WriteLine(e.Data);

        if (!e.Data.StartsWith(
            YtDlpConstants.ErrorPrefix,
            StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var errorMessage = e.Data[YtDlpConstants.ErrorPrefix.Length..];

        var eventArgs = new ErrorReceivedEventArgs
        {
            DownloadItemId = process.Id,
            ErrorMessage = errorMessage
        };

        ErrorReceived?.Invoke(this, eventArgs);
    }

    private void OnExited(
        object? sender,
        EventArgs e)
    {
        if (sender is not Process process)
        {
            return;
        }

        _downloadProcessesByProcessId.Remove(process.Id);

        //process.OutputDataReceived -= Process_OutputDataReceived;
        //process.ErrorDataReceived -= Process_ErrorDataReceived;
        //process.Exited -= Process_Exited;

        //process.Dispose();
    }

    private static string GetYtDlpFileName()
    {
        var ytDlpFileName = Path.Combine(
            AppContext.BaseDirectory,
            "Tools",
            YtDlpConstants.YtDlpExecutableName);

        if (!File.Exists(ytDlpFileName))
        {
            throw new FileNotFoundException(
                "youtube-dl executable not found.",
                ytDlpFileName);
        }

        return ytDlpFileName;
    }

    private static string GetFFmpegFileName()
    {
        var ffMpegFileName = Path.Combine(
            AppContext.BaseDirectory,
            "Tools",
            YtDlpConstants.FFmpegExecutableName);

        if (!File.Exists(ffMpegFileName))
        {
            throw new FileNotFoundException(
                "ffmpeg executable not found.",
                ffMpegFileName);
        }

        return ffMpegFileName;
    }
}
