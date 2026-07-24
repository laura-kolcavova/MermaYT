using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace MermaYT.WinUi.Views;

public sealed partial class AboutPage :
    Page,
    INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;


    private Visibility _mermaidIconDarkVisibility = Visibility.Visible;

    public Visibility MermaidIconDarkVisibility
    {
        get => _mermaidIconDarkVisibility;
        private set
        {
            _mermaidIconDarkVisibility = value;
            NotifyPropertyChanged(nameof(MermaidIconDarkVisibility));
        }
    }

    private Visibility _mermaidIconLightVisibility = Visibility.Collapsed;

    public Visibility MermaidIconLightVisibility
    {
        get => _mermaidIconLightVisibility;
        private set
        {
            _mermaidIconLightVisibility = value;
            NotifyPropertyChanged(nameof(MermaidIconLightVisibility));
        }
    }

    public AboutPage()
    {
        InitializeComponent();

        ActualThemeChanged += AboutPage_ActualThemeChanged;
    }

    private void AboutPage_ActualThemeChanged(
       FrameworkElement sender,
       object args)
    {
        UpdateMermaidIconVisibility(
            sender.ActualTheme);
    }

    private void UpdateMermaidIconVisibility(
        ElementTheme theme)
    {
        var isLight = theme == ElementTheme.Light;

        MermaidIconDarkVisibility = isLight
            ? Visibility.Collapsed
            : Visibility.Visible;

        MermaidIconLightVisibility = isLight
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    private void NotifyPropertyChanged(
        string name)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(name));
    }
}
