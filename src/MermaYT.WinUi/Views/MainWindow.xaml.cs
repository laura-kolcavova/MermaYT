using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.UI.ViewManagement;

namespace MermaYT.WinUi.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;

        SetTitleBar(AppTitleBar);

        var uiSettings = new UISettings();

        var uiForegroundColor = uiSettings.GetColorValue(UIColorType.Foreground);
        var uiBackgroundColor = uiSettings.GetColorValue(UIColorType.Background);

        AppWindow.TitleBar.ButtonForegroundColor = uiForegroundColor;
        AppWindow.TitleBar.ButtonHoverForegroundColor = uiBackgroundColor;
        AppWindow.TitleBar.ButtonPressedForegroundColor = uiBackgroundColor;

        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.PreferredMinimumWidth = 540;
            presenter.PreferredMinimumHeight = 490;
        }
        else
        {
            var newPresenter = OverlappedPresenter.Create();

            newPresenter.PreferredMinimumWidth = 540;
            newPresenter.PreferredMinimumHeight = 490;

            AppWindow.SetPresenter(newPresenter);
        }

        NavView.SelectedItem = NavView.MenuItems[0];

        Navigate(
            typeof(DownloadsPage),
            new EntranceNavigationTransitionInfo());

        // AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
    }

    private void NavView_ItemInvoked(
        NavigationView sender,
        NavigationViewItemInvokedEventArgs args)
    {
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
        var header = ((NavigationViewItem)NavView.SelectedItem)
            ?.Content
            ?.ToString();

        NavView.Header = header;
    }

    private void AppTitleBar_PaneToggleRequested(
        TitleBar sender,
        object args)
    {
        NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }

    private void Navigate(
        Type pageType,
        NavigationTransitionInfo transitionInfo)
    {
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
