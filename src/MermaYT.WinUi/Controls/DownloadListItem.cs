using MermaYT.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace MermaYT.WinUi.Controls;

public sealed partial class DownloadListItem :
    Control
{
    public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
        nameof(Item),
        typeof(DownloadItem),
        typeof(DownloadListItem),
        new PropertyMetadata(null, OnItemChanged));

    public DownloadItem? Item
    {
        get => (DownloadItem?)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public DownloadListItem()
    {
        DefaultStyleKey = typeof(DownloadListItem);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        GoToDownloadState(Item?.DownloadState ?? DownloadState.Queued);
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (DownloadListItem)d;

        if (e.OldValue is DownloadItem old)
            old.PropertyChanged -= self.OnItemPropertyChanged;

        if (e.NewValue is DownloadItem next)
            next.PropertyChanged += self.OnItemPropertyChanged;

        self.GoToDownloadState((e.NewValue as DownloadItem)?.DownloadState ?? DownloadState.Queued);
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DownloadItem.DownloadState))
            GoToDownloadState(Item?.DownloadState ?? DownloadState.Queued);
    }

    private void GoToDownloadState(DownloadState state)
    {
        VisualStateManager.GoToState(this, state.ToString(), useTransitions: true);
    }
}
