using MermaYT.Core.YouTubeDownloader.Abstractions;
using MermaYT.Core.YtDlp;
using Microsoft.Extensions.DependencyInjection;

namespace MermaYT.Core.YouTubeDownloader.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYouTubeDownloader(
        this IServiceCollection services)
    {
        services
            .AddScoped<IYouTubeDownloadManager, YtDlpDownloadAdapter>();

        return services;
    }
}
