using MermaYT.Core.YouTubeDownloader.Abstractions;
using System.Diagnostics;
using System.Text;

namespace MermaYT.Core.YouTubeDownloader.Adapters;

internal sealed class YouTubeDlAdapter
    : IYouTubeDownloadManager
{
    public async Task DownloadAsync(
        string youTubeUrl,
        OutputFormat outputFormat,
        string outputDirectory,
        CancellationToken cancellationToken = default)
    {
        var argumentsBuilder = DownloadArgumentsBuilder
            .New()
            .WithUrl(youTubeUrl)
            .WithOutputFormat(outputFormat)
            .WithOutputDirectory(outputDirectory);

        var arguments = argumentsBuilder.Build();

        var youtubeDlPath = Path.Combine(
            AppContext.BaseDirectory,
            "Tools",
            "youtube-dl.exe");

        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = youtubeDlPath,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        //process.BeginOutputReadLine();

        await process.WaitForExitAsync(cancellationToken);
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
                //stringBuilder.Append("-f bestaudio ");
                stringBuilder.Append("-x --audio-format mp3 ");
            }
            else
            {
                //stringBuilder.Append("-f bestvideo+bestaudio ");
                stringBuilder.Append("-f mp4 ");
            }

            stringBuilder.Append("--no-playlist ");

            stringBuilder.Append("--embed-thumbnail ");

            stringBuilder.Append("--add-metadata ");

            stringBuilder.Append($"-o \"{_outputDirectory}/%(title)s.%(ext)s\" ");

            stringBuilder.Append(_url);

            return stringBuilder.ToString();
        }
    }
}
