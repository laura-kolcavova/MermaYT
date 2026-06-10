using MermaYT.Core.YouTubeDownloader;

namespace MermaYT.Core.YtDlp;

internal sealed class YtDlpArgumentsBuilder
{
    private readonly List<string> _arguments;

    private YtDlpArgumentsBuilder()
    {
        _arguments = [];
    }

    public static YtDlpArgumentsBuilder New()
    {
        return new YtDlpArgumentsBuilder();
    }

    public YtDlpArgumentsBuilder NoOverwrites()
    {
        _arguments.Add("--no-overwrites");

        return this;
    }

    public YtDlpArgumentsBuilder NoPlaylist()
    {
        _arguments.Add("--no-playlist");

        return this;
    }

    public YtDlpArgumentsBuilder EmbedThumbnail()
    {
        _arguments.Add("--embed-thumbnail");

        return this;
    }

    public YtDlpArgumentsBuilder AddMetaData()
    {
        _arguments.Add("--add-metadata");

        return this;
    }

    public YtDlpArgumentsBuilder FFmpegLocation(
        string ffMpegLocation)
    {
        _arguments.Add($"--ffmpeg-location {ffMpegLocation}");

        return this;
    }

    public YtDlpArgumentsBuilder OutputTemplate(
        OutputFormat outputFormat)
    {
        if (outputFormat == OutputFormat.MP3)
        {
            _arguments.Add("-x --audio-format mp3 ");
        }
        else
        {
            _arguments.Add("-f mp4 ");
        }

        return this;
    }

    public YtDlpArgumentsBuilder OutputDirectory(
        string outputDirectory)
    {
        _arguments.Add($"-o \"{outputDirectory}/%(title)s.%(ext)s\"");

        return this;
    }

    public YtDlpArgumentsBuilder Url(
        string url)
    {
        _arguments.Add(url);

        return this;
    }

    public string Build()
    {
        if (_arguments.Count == 0)
        {
            return string.Empty;
        }

        return string.Join(' ', _arguments);
    }
}