using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MermaYT.WinUi.Controls;

public sealed partial class PageHeader :
    Control
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(PageHeader),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
        nameof(Subtitle),
        typeof(string),
        typeof(PageHeader),
        new PropertyMetadata(
            string.Empty,
            OnSubtitleChanged));

    public static readonly DependencyProperty SubtitleVisibilityProperty = DependencyProperty.Register(
        nameof(SubtitleVisibility),
        typeof(Visibility),
        typeof(PageHeader),
        new PropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty MusleIconVisibilityProperty = DependencyProperty.Register(
        nameof(MusleIconVisibility),
        typeof(Visibility),
        typeof(PageHeader),
        new PropertyMetadata(Visibility.Visible));

    public static readonly DependencyProperty MusleIconLightVisibilityProperty = DependencyProperty.Register(
        nameof(MusleIconLightVisibility),
        typeof(Visibility),
        typeof(PageHeader),
        new PropertyMetadata(Visibility.Collapsed));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public Visibility SubtitleVisibility
    {
        get => (Visibility)GetValue(SubtitleVisibilityProperty);
        private set => SetValue(SubtitleVisibilityProperty, value);
    }

    public Visibility MusleIconVisibility
    {
        get => (Visibility)GetValue(MusleIconVisibilityProperty);
        private set => SetValue(MusleIconVisibilityProperty, value);
    }

    public Visibility MusleIconLightVisibility
    {
        get => (Visibility)GetValue(MusleIconLightVisibilityProperty);
        private set => SetValue(MusleIconLightVisibilityProperty, value);
    }

    public PageHeader()
    {
        DefaultStyleKey = typeof(PageHeader);

        ActualThemeChanged += PageHeader_ActualThemeChanged;

        UpdateMusleIconVisibility(ActualTheme);
    }

    private static void OnSubtitleChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (PageHeader)d;

        var isSubtitleSet = !string.IsNullOrEmpty((string)e.NewValue);

        self.SubtitleVisibility = isSubtitleSet
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    private void PageHeader_ActualThemeChanged(
        FrameworkElement sender,
        object args)
    {
        UpdateMusleIconVisibility(sender.ActualTheme);
    }

    private void UpdateMusleIconVisibility(
        ElementTheme theme)
    {
        var isLight = theme == ElementTheme.Light;

        MusleIconVisibility = isLight
            ? Visibility.Collapsed
            : Visibility.Visible;

        MusleIconLightVisibility = isLight
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}
