using MermaYT.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace MermaYT.WinUi.Views;

public sealed partial class DownloadsPage : Page
{
    public ObservableCollection<DownloadItem> DownloadQueue { get; } = [];

    public DownloadsPage()
    {
        InitializeComponent();
    }

    private void AddButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var downloadItem = new DownloadItem();

        DownloadQueue.Add(downloadItem);
    }
}
