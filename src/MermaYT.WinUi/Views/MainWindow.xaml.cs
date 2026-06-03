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

        ExtendsContentIntoTitleBar = true;

        SetTitleBar(AppTitleBar);

        NavView.SelectedItem = NavView.MenuItems[0];

        Navigate(
            typeof(DownloadsPage),
            new EntranceNavigationTransitionInfo());

        // AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
    }

    private void NavView_ItemInvoked(
        object sender,
        NavigationViewItemInvokedEventArgs e)
    {
        var typeName = e.InvokedItemContainer
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
            e.RecommendedNavigationTransitionInfo);
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
