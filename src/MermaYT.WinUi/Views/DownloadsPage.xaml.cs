using MermaYT.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.ObjectModel;

namespace MermaYT.WinUi.Views;

public sealed partial class DownloadsPage : Page
{
    public string YouTubeUrl { get; set; } = string.Empty;

    public OutputFormat SelectedOutputFormat { get; set; } = OutputFormat.MP3;

    public string DestinationFolder { get; set; } = string.Empty;

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
            DestinationFolder = DestinationFolder
        };

        DownloadQueue.Add(downloadItem);
    }

    private async void BrowseButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var appWindow = ((App)Application.Current).Window?.AppWindow;

        if (appWindow is null)
        {
            return;
        }

        var folderPicker = new FolderPicker(appWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.Downloads,
        };

        var result = await folderPicker.PickSingleFolderAsync();

        if (result is not null)
        {
            var path = result.Path;

            DestinationFolder = path;
        }
        else
        {
            // Add your error handling here.
        }
    }
}
