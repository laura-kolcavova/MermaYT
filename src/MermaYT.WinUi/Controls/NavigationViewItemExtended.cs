using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MermaYT.WinUi.Controls;

public sealed partial class NavigationViewItemExtended :
    NavigationViewItem
{
    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
        nameof(Subtitle),
        typeof(string),
        typeof(NavigationViewItemExtended),
        new PropertyMetadata(string.Empty));

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }
}
