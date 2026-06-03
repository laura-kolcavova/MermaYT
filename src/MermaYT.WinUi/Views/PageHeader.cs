using Microsoft.UI.Xaml;

namespace MermaYT.WinUi.Views;

public record PageHeader(string Title, string? Subtitle = null)
{
    public Visibility SubtitleVisibility =>
        string.IsNullOrEmpty(Subtitle) ? Visibility.Collapsed : Visibility.Visible;
}
