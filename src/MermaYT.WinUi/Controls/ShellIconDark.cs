using MermaYT.Core.YouTubeDownloader;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace MermaYT.WinUi.Controls;

public sealed partial class ShellIconDark :
    Control
{
    public static readonly DependencyProperty FillColorBrushProperty = DependencyProperty.Register(
        nameof(FillColorBrush),
        typeof(Brush),
        typeof(ShellIconDark),
        new PropertyMetadata(
            default,
            OnFillColorBrushChanged));

    public static readonly DependencyProperty BaseColorBrushProperty = DependencyProperty.Register(
        nameof(BaseColorBrush),
        typeof(Brush),
        typeof(ShellIconDark),
        new PropertyMetadata(
            default,
            OnBaseColorBrushChanged));

    public static readonly DependencyProperty SmallCircleBrushProperty = DependencyProperty.Register(
        nameof(SmallCircleBrush),
        typeof(Brush),
        typeof(ShellIconDark),
        new PropertyMetadata(default));

    public static readonly DependencyProperty MediumCircleBrushProperty = DependencyProperty.Register(
        nameof(MediumCircleBrush),
        typeof(Brush),
        typeof(ShellIconDark),
        new PropertyMetadata(default));

    public static readonly DependencyProperty BigCircleBrushProperty = DependencyProperty.Register(
        nameof(BigCircleBrush),
        typeof(Brush),
        typeof(ShellIconDark),
        new PropertyMetadata(default));

    public static readonly DependencyProperty ShellBrushProperty = DependencyProperty.Register(
        nameof(ShellBrush),
        typeof(Brush),
        typeof(ShellIconDark),
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

    public Brush SmallCircleBrush
    {
        get => (Brush)GetValue(SmallCircleBrushProperty);
        private set => SetValue(SmallCircleBrushProperty, value);
    }

    public Brush MediumCircleBrush
    {
        get => (Brush)GetValue(MediumCircleBrushProperty);
        private set => SetValue(MediumCircleBrushProperty, value);
    }

    public Brush BigCircleBrush
    {
        get => (Brush)GetValue(BigCircleBrushProperty);
        private set => SetValue(BigCircleBrushProperty, value);
    }

    public Brush ShellBrush
    {
        get => (Brush)GetValue(ShellBrushProperty);
        private set => SetValue(ShellBrushProperty, value);
    }

    public ShellIconDark()
    {
        DefaultStyleKey = typeof(ShellIconDark);
    }

    private static void OnFillColorBrushChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (ShellIconDark)d;

        if (self.FillColorBrush is null)
        {
            return;
        }

        var newFillColorBrush = (Brush)e.NewValue;

        self.SmallCircleBrush = ComputeSmallCircleBrush(newFillColorBrush);
        self.MediumCircleBrush = ComputeMediumCircleBrush(newFillColorBrush);
        self.BigCircleBrush = ComputeBigCircleBrush(newFillColorBrush);

        if (self.BaseColorBrush is null)
        {
            return;
        }

        self.ShellBrush = ComputeShellBrush(
            newFillColorBrush,
            self.BaseColorBrush);
    }

    private static void OnBaseColorBrushChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (ShellIconDark)d;

        if (self.FillColorBrush is null ||
            self.BaseColorBrush is null)
        {
            return;
        }

        var newBaseColorBrush = (Brush)e.NewValue;

        self.ShellBrush = ComputeShellBrush(
            self.FillColorBrush,
            newBaseColorBrush);
    }

    // Fill Color + #222222 (30%)
    private static Brush ComputeSmallCircleBrush(
        Brush fillColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;

        const byte overlayR = 0x22;
        const byte overlayG = 0x22;
        const byte overlayB = 0x22;
        const double overlayOpacity = 0.3;

        var blendedColor = Color.FromArgb(
            255,
            (byte)((overlayR * overlayOpacity) + (fillColor.R * (1 - overlayOpacity))),
            (byte)((overlayG * overlayOpacity) + (fillColor.G * (1 - overlayOpacity))),
            (byte)((overlayB * overlayOpacity) + (fillColor.B * (1 - overlayOpacity))));

        return new SolidColorBrush(blendedColor);
    }

    // Fill Color (60%) + #222222 (30%)
    private static Brush ComputeMediumCircleBrush(
        Brush fillColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;

        const double fillOpacity = 0.6;

        const byte overlayR = 0x22;
        const byte overlayG = 0x22;
        const byte overlayB = 0x22;
        const double overlayOpacity = 0.3;

        var outAlpha = overlayOpacity + (fillOpacity * (1 - overlayOpacity));

        var blendedColor = Color.FromArgb(
            (byte)(outAlpha * 255),
            (byte)(((overlayR * overlayOpacity) + (fillColor.R * fillOpacity * (1 - overlayOpacity))) / outAlpha),
            (byte)(((overlayG * overlayOpacity) + (fillColor.G * fillOpacity * (1 - overlayOpacity))) / outAlpha),
            (byte)(((overlayB * overlayOpacity) + (fillColor.B * fillOpacity * (1 - overlayOpacity))) / outAlpha));

        return new SolidColorBrush(blendedColor);
    }

    // Fill Color + #222222 (30%)
    private static Brush ComputeBigCircleBrush(
        Brush fillColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;

        const byte overlayR = 0x22;
        const byte overlayG = 0x22;
        const byte overlayB = 0x22;
        const double overlayOpacity = 0.3;

        var blendedColor = Color.FromArgb(
            255,
            (byte)((overlayR * overlayOpacity) + (fillColor.R * (1 - overlayOpacity))),
            (byte)((overlayG * overlayOpacity) + (fillColor.G * (1 - overlayOpacity))),
            (byte)((overlayB * overlayOpacity) + (fillColor.B * (1 - overlayOpacity))));

        return new SolidColorBrush(blendedColor);
    }

    // Base Color + Fill Color (10%)
    private static Brush ComputeShellBrush(
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
