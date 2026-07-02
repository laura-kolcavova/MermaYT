using MermaYT.WinUi.Controls;
using MermaYT.WinUi.Models;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;

namespace MermaYT.WinUi.Views;

public sealed partial class MainWindow : Window
{
    private static readonly Dictionary<string, Type> _pageTypesByTag = new()
    {
        { "DownloadsPage", typeof(DownloadsPage) },
        { "AboutPage", typeof(AboutPage) }
    };

    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;

        SetTitleBar(AppTitleBar);

        //var iconId = IconService.GetApplicationIconId();

        //AppWindow.SetIcon(iconId);
        //AppWindow.SetTaskbarIcon(iconId);

        if (Content is FrameworkElement rootElement)
        {
            rootElement.ActualThemeChanged += RootElement_ActualThemeChanged;

            UpdateAppTitleBarColors(rootElement.ActualTheme);
        }

        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.PreferredMinimumWidth = 540;
            presenter.PreferredMinimumHeight = 490;
        }

        NavView.SelectedItem = NavView.MenuItems[0];

        Navigate(
            typeof(DownloadsPage),
            new EntranceNavigationTransitionInfo());
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

        var tag = args.InvokedItemContainer
            ?.Tag
            ?.ToString();

        if (tag is null)
        {
            return;
        }

        if (!_pageTypesByTag.TryGetValue(
            tag,
            out var pageType))
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
        var title = string.Empty;

        var subtitle = string.Empty;

        if (NavView.SelectedItem is NavigationViewItem navigationViewItem)
        {
            title = navigationViewItem.Content.ToString() ?? string.Empty;
        }

        if (NavView.SelectedItem is NavigationViewItemExtended navigationViewItemExtended)
        {
            subtitle = navigationViewItemExtended.Subtitle;
        }

        var navigationHeaderInfo = new NavigationHeaderInfoModel(
            Title: title,
            Subtitle: subtitle);

        NavView.Header = navigationHeaderInfo;
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
        var isLight = theme == ElementTheme.Light;

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
