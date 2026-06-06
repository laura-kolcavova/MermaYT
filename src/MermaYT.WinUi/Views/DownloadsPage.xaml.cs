using MermaYT.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace MermaYT.WinUi.Views;

public sealed partial class DownloadsPage : Page
{
    public string YouTubeUrl { get; set; } = string.Empty;

    public OutputFormat SelectedOutputFormat { get; set; } = OutputFormat.MP3;

    public ObservableCollection<DownloadItem> DownloadQueue { get; } = [];

    public DownloadsPage()
    {
        InitializeComponent();
    }

    private void AddButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var downloadItem = new DownloadItem()
        {
            Url = YouTubeUrl,
            Title = YouTubeUrl,
            OutputFormat = SelectedOutputFormat,
        };

        DownloadQueue.Add(downloadItem);
    }
}
