using MermaYT.Core.YouTubeDownloader;
using MermaYT.Core.YouTubeDownloader.Abstractions;
using MermaYT.Core.YouTubeDownloader.Events;
using System.Diagnostics;

namespace MermaYT.Core.YtDlp;

internal sealed class YtDlpDownloadAdapter :
    IYouTubeDownloadManager
//IDisposable
{
    public event EventHandler<ProgressUpdatedEventArgs>? ProgressUpdated;

    public event EventHandler<ConvertingStartedEventArgs>? ConvertingStarted;

    public event EventHandler<FailedEventArgs>? Failed;

    public event EventHandler<CompletedEventArgs>? Completed;

    private readonly Dictionary<int, Process> _downloadProcessesByProcessId = [];

    private bool _disposed;

    //~YtDlpDownloadAdapter()
    //{
    //    //Dispose(false);
    //}

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

        var process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true,
        };

        process.OutputDataReceived += OnOutputDataReceived;
        process.ErrorDataReceived += OnErrorDataReceived;
        process.Exited += OnExited;

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        _downloadProcessesByProcessId.Add(process.Id, process);

        return process.Id;
    }

    public void Dispose()
    {
        //Dispose(true);

        //GC.SuppressFinalize(this);
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

            var progressUpdatedEventArgs = new ProgressUpdatedEventArgs
            {
                DownloadItemId = process.Id,
                ProgressPercentage = parsedPercent,
                DownloadedBytes = parsedDownloadedBytes,
                TotalBytes = parsedTotalBytes,
                Title = title
            };

            ProgressUpdated?.Invoke(this, progressUpdatedEventArgs);

            return;
        }

        if (e.Data.StartsWith(
          YtDlpConstants.ConvertingPrefix,
          StringComparison.OrdinalIgnoreCase))
        {
            var convertingStartedEventArgs = new ConvertingStartedEventArgs
            {
                DownloadItemId = process.Id
            };

            ConvertingStarted?.Invoke(this, convertingStartedEventArgs);

            return;
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

        var eventArgs = new FailedEventArgs
        {
            DownloadItemId = process.Id,
            ErrorMessage = errorMessage
        };

        Failed?.Invoke(this, eventArgs);
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

        if (process.ExitCode < 0)
        {
            var failedEventArgs = new FailedEventArgs
            {
                DownloadItemId = process.Id,
                ErrorMessage = $"Process exited with code {process.ExitCode}."
            };

            Failed?.Invoke(this, failedEventArgs);

            return;
        }

        var completedEventArgs = new CompletedEventArgs
        {
            DownloadItemId = process.Id
        };

        Completed?.Invoke(this, completedEventArgs);
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

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed state (managed objects).
            // ...

            //foreach (var process in _downloadProcessesByProcessId.Values)
            //{
            //    process.OutputDataReceived -= OnOutputDataReceived;
            //    process.ErrorDataReceived -= OnErrorDataReceived;
            //    process.Exited -= OnExited;

            //    if (!process.HasExited)
            //    {
            //        process.Kill();
            //    }

            //    process.Dispose();
            //}

            //_downloadProcessesByProcessId.Clear();
        }

        // Free unmanaged resources.
        // ...

        _disposed = true;
    }
}
