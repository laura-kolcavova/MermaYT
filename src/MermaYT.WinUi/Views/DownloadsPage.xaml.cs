using MermaYT.Core;
using MermaYT.WinUi.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace MermaYT.WinUi.Views;

public sealed partial class DownloadsPage :
    Page,
    INotifyPropertyChanged
{
    private string _youTubeUrl = string.Empty;

    public string YouTubeUrl
    {
        get => _youTubeUrl;
        set
        {
            _youTubeUrl = value;
            NotifyPropertyChanged();
            CheckAddButtonIsEnabled();
        }
    }

    public OutputFormat SelectedOutputFormat { get; set; } = OutputFormat.MP3;

    private string _destinationFolder = Environment.GetFolderPath(
        Environment.SpecialFolder.Desktop);

    public string DestinationFolder
    {
        get => _destinationFolder;
        set
        {
            _destinationFolder = value;
            NotifyPropertyChanged();
            CheckAddButtonIsEnabled();
        }
    }

    private bool _addButtonIsEnabled = false;

    public bool AddButtonIsEnabled
    {
        get => _addButtonIsEnabled;
        private set
        {
            _addButtonIsEnabled = value;
            NotifyPropertyChanged();
        }
    }

    public ObservableCollection<DownloadItem> DownloadQueue { get; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    public DownloadsPage()
    {
        InitializeComponent();

        CheckAddButtonIsEnabled();
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

        var suggestedFolder = Directory.Exists(DestinationFolder)
            ? DestinationFolder
            : Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop);

        var folderPicker = new FolderPicker(appWindow.Id)
        {
            SuggestedFolder = suggestedFolder,
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

    private void CheckAddButtonIsEnabled()
    {
        var isEnabled = true;

        if (string.IsNullOrEmpty(YouTubeUrl))
        {
            isEnabled = false;
        }

        if (string.IsNullOrEmpty(DestinationFolder) ||
            !Directory.Exists(DestinationFolder))
        {
            isEnabled = false;
        }

        if (AddButtonIsEnabled != isEnabled)
        {
            AddButtonIsEnabled = isEnabled;
        }
    }

    private void NotifyPropertyChanged(
        [CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
