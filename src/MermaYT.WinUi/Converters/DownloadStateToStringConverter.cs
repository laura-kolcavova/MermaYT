using MermaYT.Core;
using Microsoft.UI.Xaml.Data;
using System;

namespace MermaYT.WinUi.Converters;

public sealed class DownloadStateToStringConverter :
    IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        if (value is not DownloadState state)
        {
            return string.Empty;
        }

        return state switch
        {
            DownloadState.Queued => "Queued",
            DownloadState.Processing => "Processing",
            DownloadState.Downloading => "Downloading",
            DownloadState.Converting => "Converting",
            DownloadState.Paused => "Paused",
            DownloadState.Completed => "Completed",
            DownloadState.Failed => "Failed",
            DownloadState.Cancelled => "Cancelled",
            _ => string.Empty
        };
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        string language)
        => throw new NotImplementedException();
}
