using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace MermaYT.WinUi.Controls;

public sealed partial class NavigationViewExtended :
    NavigationView
{
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(NavigationViewExtended),
        new PropertyMetadata(null, OnImageSourceChanged));

    AcrylicBrush? oldNavigationViewDefaultPaneBackground;

    private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (NavigationViewExtended)d;

        if (e.NewValue is ImageSource source)
        {
            control.Resources["NavigationViewExpandedPaneBackground"] = new ImageBrush
            {
                ImageSource = source,
                Stretch = Stretch.UniformToFill,
            };
        }
        else
        {
            control.Resources.Remove("NavigationViewExpandedPaneBackground");
        }
    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
}