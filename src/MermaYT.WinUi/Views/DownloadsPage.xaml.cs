using MermaYT.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MermaYT.WinUi.Views;

public sealed partial class DownloadsPage :
    Page,
    INotifyPropertyChanged
{
    public string YouTubeUrl { get; set; } = string.Empty;

    public OutputFormat SelectedOutputFormat { get; set; } = OutputFormat.MP3;

    public string DestinationFolder { get; set; } = Environment.GetFolderPath(
        Environment.SpecialFolder.Desktop);

    public ObservableCollection<DownloadItem> DownloadQueue { get; } = [];

    public DownloadsPage()
    {
        InitializeComponent();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

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
            SuggestedFolder = DestinationFolder,
        };

        var result = await folderPicker.PickSingleFolderAsync();

        if (result is not null)
        {
            var path = result.Path;

            DestinationFolder = path;
            NotifyPropertyChanged(nameof(DestinationFolder));
        }
        else
        {
            // Add your error handling here.
        }
    }

    private void NotifyPropertyChanged(
        [CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
