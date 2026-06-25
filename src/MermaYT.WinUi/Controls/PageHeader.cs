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

    public PageHeader()
    {
        DefaultStyleKey = typeof(PageHeader);
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
}
