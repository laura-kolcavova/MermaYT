using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MermaYT.WinUi.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();

        InitializeThemeComboBox();
    }

    private void InitializeThemeComboBox()
    {
        var rootElement = XamlRoot?.Content as FrameworkElement;

        if (rootElement is null)
        {
            ThemeComboBox.SelectedIndex = 0;

            return;
        }

        ThemeComboBox.SelectedIndex = rootElement.RequestedTheme switch
        {
            ElementTheme.Light => 1,
            ElementTheme.Dark => 2,
            _ => 0,
        };
    }

    private void ThemeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        var rootElement = XamlRoot?.Content as FrameworkElement;

        if (rootElement is null)
        {
            return;
        }

        var tag = ((ComboBoxItem)ThemeComboBox.SelectedItem)
            ?.Tag
            ?.ToString();

        rootElement.RequestedTheme = tag switch
        {
            "Light" => ElementTheme.Light,
            "Dark" => ElementTheme.Dark,
            _ => ElementTheme.Default,
        };
    }
}
