using MermaYT.Core.YouTubeDownloader.Abstractions;
using MermaYT.Core.YouTubeDownloader.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace MermaYT.Core.YouTubeDownloader.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYouTubeDownloader(
        this IServiceCollection services)
    {
        services
            .AddScoped<IYouTubeDownloadManager, YouTubeDlAdapter>();

        return services;
    }
}
