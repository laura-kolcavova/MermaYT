using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace MermaYT.WinUi.Controls;

public sealed partial class ShellIconLight :
    Control
{
    public static readonly DependencyProperty FillColorBrushProperty = DependencyProperty.Register(
         nameof(FillColorBrush),
         typeof(Brush),
         typeof(ShellIconLight),
         new PropertyMetadata(
             default,
             OnFillColorBrushChanged));

    public static readonly DependencyProperty BaseColorBrushProperty = DependencyProperty.Register(
        nameof(BaseColorBrush),
        typeof(Brush),
        typeof(ShellIconLight),
        new PropertyMetadata(
            default,
            OnBaseColorBrushChanged));

    public static readonly DependencyProperty SmallCircleBrushProperty = DependencyProperty.Register(
        nameof(SmallCircleBrush),
        typeof(Brush),
        typeof(ShellIconLight),
        new PropertyMetadata(default));

    public static readonly DependencyProperty MediumCircleBrushProperty = DependencyProperty.Register(
        nameof(MediumCircleBrush),
        typeof(Brush),
        typeof(ShellIconLight),
        new PropertyMetadata(default));

    public static readonly DependencyProperty BigCircleBrushProperty = DependencyProperty.Register(
        nameof(BigCircleBrush),
        typeof(Brush),
        typeof(ShellIconLight),
        new PropertyMetadata(default));

    public static readonly DependencyProperty ShellBrushProperty = DependencyProperty.Register(
        nameof(ShellBrush),
        typeof(Brush),
        typeof(ShellIconLight),
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

    public ShellIconLight()
    {
        DefaultStyleKey = typeof(ShellIconLight);
    }

    private static void OnFillColorBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
    {
        var self = (ShellIconLight)d;

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
        var self = (ShellIconLight)d;

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

    // Fill Color (100%)
    private static Brush ComputeSmallCircleBrush(
        Brush fillColorBrush)
    {
        return fillColorBrush;
    }

    // Fill Color (60%)
    private static Brush ComputeMediumCircleBrush(
        Brush fillColorBrush)
    {
        var fillColor = ((SolidColorBrush)fillColorBrush).Color;

        const double fillOpacity = 0.6;

        var blendedColor = Color.FromArgb(
           (byte)(fillOpacity * 255),
           fillColor.R,
           fillColor.G,
           fillColor.B);

        return new SolidColorBrush(blendedColor);
    }

    // Fill Color (100%)
    private static Brush ComputeBigCircleBrush(
        Brush fillColorBrush)
    {
        return fillColorBrush;
    }

    // Base Color + Fill Color (10%)
    private static Brush ComputeShellBrush(
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
