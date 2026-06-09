using MermaYT.Core.YouTubeDownloader.Abstractions;
using MermaYT.Core.YouTubeDownloader.Events;
using System.Diagnostics;
using System.Text;

namespace MermaYT.Core.YouTubeDownloader.Adapters;

internal sealed class YouTubeDlAdapter
    : IYouTubeDownloadAdapter
{
    private const string youtubeDlExecutableName = "youtube-dl.exe";

    private const string errorPrefix = "ERROR: ";

    private readonly Dictionary<int, Process> _downloadProcessesByProcessId = [];

    public event EventHandler<DownloadErrorEventArgs>? DownloadErrorReceived;

    public void CancelDownload(
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
        var youtubeDlPath = Path.Combine(
            AppContext.BaseDirectory,
            "Tools",
            youtubeDlExecutableName);

        if (!File.Exists(youtubeDlPath))
        {
            throw new FileNotFoundException(
                "youtube-dl executable not found.",
                youtubeDlPath);
        }

        var argumentsBuilder = DownloadArgumentsBuilder
            .New()
            .WithUrl(youTubeUrl)
            .WithOutputFormat(outputFormat)
            .WithOutputDirectory(outputDirectory);

        var arguments = argumentsBuilder.Build();

        var startInfo = new ProcessStartInfo
        {
            FileName = youtubeDlPath,
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
            errorPrefix,
            StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var errorMessage = e.Data[errorPrefix.Length..];

        var eventArgs = new DownloadErrorEventArgs
        {
            DownloadItemId = process.Id,
            ErrorMessage = errorMessage
        };

        DownloadErrorReceived?.Invoke(this, eventArgs);
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

    private sealed class DownloadArgumentsBuilder
    {
        private string _url = string.Empty;

        private OutputFormat _outputFormat = OutputFormat.MP4;

        private string _outputDirectory = string.Empty;

        private DownloadArgumentsBuilder()
        {
        }

        public static DownloadArgumentsBuilder New()
        {
            return new DownloadArgumentsBuilder();
        }

        public DownloadArgumentsBuilder WithUrl(
            string url)
        {
            _url = url;

            return this;
        }

        public DownloadArgumentsBuilder WithOutputDirectory(
            string outputDirectory)
        {
            _outputDirectory = outputDirectory;

            return this;
        }

        public DownloadArgumentsBuilder WithOutputFormat(
            OutputFormat outputFormat)
        {
            _outputFormat = outputFormat;

            return this;
        }

        public string Build()
        {
            if (string.IsNullOrEmpty(_url))
            {
                throw new InvalidOperationException("URL must be provided.");
            }

            if (string.IsNullOrEmpty(_outputDirectory))
            {
                throw new InvalidOperationException("Output directory must be provided.");
            }

            var stringBuilder = new StringBuilder();

            if (_outputFormat == OutputFormat.MP3)
            {
                stringBuilder.Append("-x --audio-format mp3 ");
            }
            else
            {
                stringBuilder.Append("-f mp4 ");
            }

            stringBuilder.Append("--no-overwrites ");

            stringBuilder.Append("--no-playlist ");

            stringBuilder.Append("--embed-thumbnail ");

            stringBuilder.Append("--add-metadata ");

            stringBuilder.Append($"-o \"{_outputDirectory}/%(title)s.%(ext)s\" ");

            stringBuilder.Append(_url);

            return stringBuilder.ToString();
        }
    }
}
