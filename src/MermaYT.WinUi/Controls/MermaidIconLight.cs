using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace MermaYT.WinUi.Controls;

public sealed partial class MermaidIconLight :
    Control
{
    public static readonly DependencyProperty FillColorBrushProperty = DependencyProperty.Register(
        nameof(FillColorBrush),
        typeof(Brush),
        typeof(MermaidIconLight),
        new PropertyMetadata(
            default,
            OnFillColorBrushChanged));

    public static readonly DependencyProperty BaseColorBrushProperty = DependencyProperty.Register(
        nameof(BaseColorBrush),
        typeof(Brush),
        typeof(MermaidIconLight),
        new PropertyMetadata(
            default,
            OnBaseColorBrushChanged));

    public static readonly DependencyProperty SunBrushProperty = DependencyProperty.Register(
        nameof(SunBrush),
        typeof(Brush),
        typeof(MermaidIconLight),
        new PropertyMetadata(default));

    public static readonly DependencyProperty MermaidBrushProperty = DependencyProperty.Register(
        nameof(MermaidBrush),
        typeof(Brush),
        typeof(MermaidIconLight),
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

    public MermaidIconLight()
    {
        DefaultStyleKey = typeof(MermaidIconLight);
    }

    private static void OnFillColorBrushChanged(
       DependencyObject d,
       DependencyPropertyChangedEventArgs e)
    {
        var self = (MermaidIconLight)d;

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
        var self = (MermaidIconLight)d;

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

    // Fill Color + #ffffff (70%)
    private static Brush ComputeSunBrush(
        Brush fillColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;

        const byte overlayR = 0xff;
        const byte overlayG = 0xff;
        const byte overlayB = 0xff;
        const double overlayOpacity = 0.7;

        var blendedColor = Color.FromArgb(
            255,
            (byte)((overlayR * overlayOpacity) + (fillColor.R * (1 - overlayOpacity))),
            (byte)((overlayG * overlayOpacity) + (fillColor.G * (1 - overlayOpacity))),
            (byte)((overlayB * overlayOpacity) + (fillColor.B * (1 - overlayOpacity))));

        return new SolidColorBrush(blendedColor);
    }

    // Base Color + Fill Color (10%)
    private static Brush ComputeMermaidBrush(
        Brush fillColorBrush,
        Brush baseColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;
        var baseColor = ((SolidColorBrush)baseColorBrush).Color;

        const double fillOpacity = 0.1;

        var blendedColor = Color.FromArgb(
            255,
            (byte)((fillColor.R * fillOpacity) + (baseColor.R * (1 - fillOpacity))),
            (byte)((fillColor.G * fillOpacity) + (baseColor.G * (1 - fillOpacity))),
            (byte)((fillColor.B * fillOpacity) + (baseColor.B * (1 - fillOpacity))));

        return new SolidColorBrush(blendedColor);
    }
}
