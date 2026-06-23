using MermaYT.Core.YouTubeDownloader;
using Microsoft.UI.Xaml.Data;
using System;

namespace MermaYT.WinUi.Converters;

public sealed class OutputFormatToIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
        => value is OutputFormat format
        ? (int)format
        : 0;

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => value is int index
        ? (OutputFormat)index
        : OutputFormat.MP3;
}
