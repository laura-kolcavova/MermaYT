using MermaYT.Core;
using Microsoft.UI.Xaml.Data;
using System;

namespace MermaYT.WinUi.Converters;

public sealed class OutputFormatToGlyphConverter :
    IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        if (value is not OutputFormat format)
        {
            return string.Empty;
        }

        return format switch
        {
            OutputFormat.MP3 => "MP3",  // Audio
            OutputFormat.MP4 => "MP4",  // Movie
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
