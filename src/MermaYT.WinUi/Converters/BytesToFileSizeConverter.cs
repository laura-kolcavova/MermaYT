using Microsoft.UI.Xaml.Data;
using System;

namespace MermaYT.WinUi.Converters;

public sealed class BytesToFileSizeConverter :
    IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        if (value is not long bytes || bytes < 0)
        {
            return "?";
        }

        if (bytes < 1_024)
        {
            return $"{bytes} B";
        }

        if (bytes < 1_024 * 1_024)
        {
            return $"{bytes / 1_024.0:F1} KB";
        }

        return $"{bytes / (1_024.0 * 1_024.0):F1} MB";
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        string language)
        => throw new NotImplementedException();
}
