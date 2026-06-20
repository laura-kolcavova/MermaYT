using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MermaYT.WinUi.Controls;

internal sealed class NavigationViewItemExtended :
    NavigationViewItem
{
    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
        nameof(Subtitle),
        typeof(string),
        typeof(NavigationViewItemExtended),
        new PropertyMetadata(default(string)));

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }
}
