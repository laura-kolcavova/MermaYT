using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace MermaYT.WinUi.Controls;

public sealed partial class MermaidIconDark :
    Control
{
    public static readonly DependencyProperty FillColorBrushProperty = DependencyProperty.Register(
        nameof(FillColorBrush),
        typeof(Brush),
        typeof(MermaidIconDark),
        new PropertyMetadata(
            default,
            OnFillColorBrushChanged));

    public static readonly DependencyProperty BaseColorBrushProperty = DependencyProperty.Register(
        nameof(BaseColorBrush),
        typeof(Brush),
        typeof(MermaidIconDark),
        new PropertyMetadata(
            default,
            OnBaseColorBrushChanged));

    public static readonly DependencyProperty SunBrushProperty = DependencyProperty.Register(
        nameof(SunBrush),
        typeof(Brush),
        typeof(MermaidIconDark),
        new PropertyMetadata(default));

    public static readonly DependencyProperty MermaidBrushProperty = DependencyProperty.Register(
        nameof(MermaidBrush),
        typeof(Brush),
        typeof(MermaidIconDark),
        new PropertyMetadata(default));

    public Brush FillColorBrush
    {
        get => (Brush)GetValue(FillColorBrushProperty);
        set => SetValue(FillColorBrushProperty, value);
    }

    public Brush BaseColorBrush
    {
        get => (Brush)GetValue(BaseColorBrushProperty);
        set => SetValue(BaseColorBrushProperty, value);
    }

    public Brush SunBrush
    {
        get => (Brush)GetValue(SunBrushProperty);
        private set => SetValue(SunBrushProperty, value);
    }

    public Brush MermaidBrush
    {
        get => (Brush)GetValue(MermaidBrushProperty);
        private set => SetValue(MermaidBrushProperty, value);
    }

    public MermaidIconDark()
    {
        DefaultStyleKey = typeof(MermaidIconDark);
    }

    private static void OnFillColorBrushChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (MermaidIconDark)d;

        if (self.FillColorBrush is null)
        {
            return;
        }

        var newFillColorBrush = (Brush)e.NewValue;

        self.SunBrush = ComputeSunBrush(newFillColorBrush);

        if (self.BaseColorBrush is null)
        {
            return;
        }

        self.MermaidBrush = ComputeMermaidBrush(
            newFillColorBrush,
            self.BaseColorBrush);
    }

    private static void OnBaseColorBrushChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (MermaidIconDark)d;

        if (self.FillColorBrush is null ||
            self.BaseColorBrush is null)
        {
            return;
        }

        var newBaseColorBrush = (Brush)e.NewValue;

        self.MermaidBrush = ComputeMermaidBrush(
            self.FillColorBrush,
            newBaseColorBrush);
    }

    // Fill Color + #222222 (80%)
    private static Brush ComputeSunBrush(
        Brush fillColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;

        const byte overlayR = 0x22;
        const byte overlayG = 0x22;
        const byte overlayB = 0x22;
        const double overlayOpacity = 0.8;

        var blendedColor = Color.FromArgb(
            255,
            (byte)((overlayR * overlayOpacity) + (fillColor.R * (1 - overlayOpacity))),
            (byte)((overlayG * overlayOpacity) + (fillColor.G * (1 - overlayOpacity))),
            (byte)((overlayB * overlayOpacity) + (fillColor.B * (1 - overlayOpacity))));

        return new SolidColorBrush(blendedColor);
    }

    // Base Color + Fill Color (20%) + #222222 (30%)
    private static Brush ComputeMermaidBrush(
        Brush fillColorBrush,
        Brush baseColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;
        var baseColor = ((SolidColorBrush)baseColorBrush).Color;

        const double fillOpacity = 0.2;

        const byte overlayR = 0x22;
        const byte overlayG = 0x22;
        const byte overlayB = 0x22;
        const double overlayOpacity = 0.3;

        var midR = (fillColor.R * fillOpacity) + (baseColor.R * (1 - fillOpacity));
        var midG = (fillColor.G * fillOpacity) + (baseColor.G * (1 - fillOpacity));
        var midB = (fillColor.B * fillOpacity) + (baseColor.B * (1 - fillOpacity));

        var blendedColor = Color.FromArgb(
            255,
            (byte)((overlayR * overlayOpacity) + (midR * (1 - overlayOpacity))),
            (byte)((overlayG * overlayOpacity) + (midG * (1 - overlayOpacity))),
            (byte)((overlayB * overlayOpacity) + (midB * (1 - overlayOpacity))));

        return new SolidColorBrush(blendedColor);
    }
}
