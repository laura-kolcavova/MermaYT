using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace MermaYT.WinUi.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        InitializeAppWindow();

        InitializeAppTitleBar();

        NavView.SelectedItem = NavView.MenuItems[0];

        Navigate(
            typeof(DownloadsPage),
            new EntranceNavigationTransitionInfo());
    }

    private void InitializeAppWindow()
    {
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.PreferredMinimumWidth = 540;
            presenter.PreferredMinimumHeight = 490;
        }
    }

    private void InitializeAppTitleBar()
    {
        ExtendsContentIntoTitleBar = true;

        SetTitleBar(AppTitleBar);

        if (Content is FrameworkElement rootElement)
        {
            rootElement.ActualThemeChanged += RootElement_ActualThemeChanged;

            UpdateAppTitleBarColors(rootElement.ActualTheme);
        }

        // AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
    }

    private void RootElement_ActualThemeChanged(
        FrameworkElement sender,
        object args)
    {
        UpdateAppTitleBarColors(sender.ActualTheme);
    }

    private void NavView_ItemInvoked(
        NavigationView sender,
        NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            Navigate(
                typeof(SettingsPage),
                args.RecommendedNavigationTransitionInfo);

            return;
        }

        var typeName = args.InvokedItemContainer
            ?.Tag
            ?.ToString();

        if (typeName is null)
        {
            return;
        }

        var pageType = Type.GetType(typeName);

        if (pageType is null)
        {
            return;
        }

        Navigate(
            pageType,
            args.RecommendedNavigationTransitionInfo);
    }

    private void NavFrame_Navigated(
        object sender,
        NavigationEventArgs e)
    {
        NavView.Header = (NavView.SelectedItem as NavigationViewItem)
            ?.Content
            ?.ToString()
            ?? string.Empty;
    }

    private void AppTitleBar_PaneToggleRequested(
        TitleBar sender,
        object args)
    {
        NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }

    private void UpdateAppTitleBarColors(
        ElementTheme theme)
    {
        bool isLight = theme == ElementTheme.Light;

        AppWindow.TitleBar.ButtonForegroundColor = isLight
            ? Colors.Black
            : Colors.White;
        AppWindow.TitleBar.ButtonHoverForegroundColor = isLight
            ? Colors.White
            : Colors.Black;
        AppWindow.TitleBar.ButtonPressedForegroundColor = isLight
            ? Colors.White
            : Colors.Black;
    }

    private void Navigate(
        Type pageType,
        NavigationTransitionInfo transitionInfo)
    {
        if (pageType is null)
        {
            return;
        }

        var currentPageType = NavFrame.CurrentSourcePageType;

        if (currentPageType == pageType)
        {
            return;
        }

        NavFrame.Navigate(
            pageType,
            null,
            transitionInfo);
    }
}
