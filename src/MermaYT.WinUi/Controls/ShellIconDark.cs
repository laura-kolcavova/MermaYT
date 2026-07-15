using MermaYT.Core.YouTubeDownloader;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace MermaYT.WinUi.Controls;

public sealed partial class ShellIconDark :
    Control
{
    public static readonly DependencyProperty FillBrushProperty = DependencyProperty.Register(
        nameof(FillBrush),
        typeof(Brush),
        typeof(ShellIconDark),
        new PropertyMetadata(
            default,
            OnFillBrushChanged));

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

    public static readonly DependencyProperty SecondaryColorBrushProperty = DependencyProperty.Register(
        nameof(SecondaryColorBrush),
        typeof(Brush),
        typeof(ShellIconDark),
        new PropertyMetadata(
            default,
            OnSecondaryColorBrushChanged));

    public Brush FillBrush
    {
        get => (Brush)GetValue(FillBrushProperty);
        set => SetValue(FillBrushProperty, value);
    }

    public Brush SecondaryColorBrush
    {
        get => (Brush)GetValue(SecondaryColorBrushProperty);
        set => SetValue(SecondaryColorBrushProperty, value);
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

    private static void OnFillBrushChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (ShellIconDark)d;

        if (self.FillBrush is null)
        {
            return;
        }

        var newFillBrush = (Brush)e.NewValue;

        self.SmallCircleBrush = ComputeSmallCircleBrush(newFillBrush);
        self.MediumCircleBrush = ComputeMediumCircleBrush(newFillBrush);
        self.BigCircleBrush = ComputeBigCircleBrush(newFillBrush);

        if (self.SecondaryColorBrush is null)
        {
            return;
        }

        self.ShellBrush = ComputeShellBrush(
            newFillBrush,
            self.SecondaryColorBrush);
    }

    private static void OnSecondaryColorBrushChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (ShellIconDark)d;

        if (self.FillBrush is null ||
            self.SecondaryColorBrush is null)
        {
            return;
        }

        var newSecondaryColorBrush = (Brush)e.NewValue;

        self.ShellBrush = ComputeShellBrush(
            self.FillBrush,
            newSecondaryColorBrush);
    }

    private static Brush ComputeSmallCircleBrush(
        Brush fillBrush)
    {
        var fillColor = ((SolidColorBrush)fillBrush).Color;

        const byte overlayR = 0x22;
        const byte overlayG = 0x22;
        const byte overlayB = 0x22;
        const double overlayOpacity = 0.3;

        var blendedColor = Color.FromArgb(
            fillColor.A,
            (byte)((overlayR * overlayOpacity) + (fillColor.R * (1 - overlayOpacity))),
            (byte)((overlayG * overlayOpacity) + (fillColor.G * (1 - overlayOpacity))),
            (byte)((overlayB * overlayOpacity) + (fillColor.B * (1 - overlayOpacity))));

        return new SolidColorBrush(blendedColor);
    }

    private static Brush ComputeMediumCircleBrush(
        Brush fillBrush)
    {
        var fillColor = ((SolidColorBrush)fillBrush).Color;

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

    private static Brush ComputeBigCircleBrush(
        Brush fillBrush)
    {
        var fillColor = ((SolidColorBrush)fillBrush).Color;

        const byte overlayR = 0x22;
        const byte overlayG = 0x22;
        const byte overlayB = 0x22;
        const double overlayOpacity = 0.3;

        var blendedColor = Color.FromArgb(
            fillColor.A,
            (byte)((overlayR * overlayOpacity) + (fillColor.R * (1 - overlayOpacity))),
            (byte)((overlayG * overlayOpacity) + (fillColor.G * (1 - overlayOpacity))),
            (byte)((overlayB * overlayOpacity) + (fillColor.B * (1 - overlayOpacity))));

        return new SolidColorBrush(blendedColor);
    }

    private static Brush ComputeShellBrush(
        Brush fillBrush,
        Brush secondaryColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillBrush).Color;
        var baseColor = ((SolidColorBrush)secondaryColorBrush).Color;

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
