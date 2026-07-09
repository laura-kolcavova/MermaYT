using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace MermaYT.WinUi.Controls;

public sealed partial class ShellIconLight :
    Control
{
    public static readonly DependencyProperty FillBrushProperty = DependencyProperty.Register(
        nameof(FillBrush),
        typeof(Brush),
        typeof(ShellIconLight),
        new PropertyMetadata(default));

    public Brush FillBrush
    {
        get => (Brush)GetValue(FillBrushProperty);
        set => SetValue(FillBrushProperty, value);
    }

    public ShellIconLight()
    {
        DefaultStyleKey = typeof(ShellIconLight);
    }
}
