using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.UI;

namespace MermaYT.WinUi.Controls;

public sealed partial class MusleIcon :
    Control
{
    private static readonly Regex BigCircleFillRegex = new(
        """(<g\s+id="bg_100_big"[^>]*>\s*<circle\b[^>]*\bstyle="fill:)[^;"]+(;?")""",
        RegexOptions.Singleline);

    private Image? _musleImage;

    public MusleIcon()
    {
        DefaultStyleKey = typeof(MusleIcon);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _musleImage = GetTemplateChild("MusleImage") as Image;

        _ = LoadMusleImageAsync();
    }

    private async System.Threading.Tasks.Task LoadMusleImageAsync()
    {
        if (_musleImage is null)
        {
            return;
        }

        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/musle.svg"));
        var svg = await FileIO.ReadTextAsync(file);

        var color = (Foreground as SolidColorBrush)?.Color ?? Colors.Black;
        var coloredSvg = BigCircleFillRegex.Replace(svg, match => $"{match.Groups[1].Value}{ToHex(color)}{match.Groups[2].Value}");

        var svgImageSource = new SvgImageSource();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(coloredSvg));
        await svgImageSource.SetSourceAsync(stream.AsRandomAccessStream());

        _musleImage.Source = svgImageSource;
    }

    private static string ToHex(Color color) =>
        $"#{color.R:X2}{color.G:X2}{color.B:X2}";
}
