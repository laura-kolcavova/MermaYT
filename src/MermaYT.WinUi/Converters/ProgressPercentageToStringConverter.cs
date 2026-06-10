using Microsoft.UI.Xaml.Data;
using System;

namespace MermaYT.WinUi.Converters;

internal sealed class ProgressPercentageToStringConverter :
    IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        string language)
    {
        if (value is not float progress)
        {
            return "0%";
        }

        if (progress < 0)
        {
            return "0%";
        }

        if (progress > 100)
        {
            return "100%";
        }

        return $"{(int)Math.Floor(progress)}%";
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        string language)
        => throw new NotImplementedException();
}
