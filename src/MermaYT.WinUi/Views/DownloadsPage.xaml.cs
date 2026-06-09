using MermaYT.Core.YouTubeDownloader;
using MermaYT.Core.YouTubeDownloader.Abstractions;
using MermaYT.WinUi.Controls;
using MermaYT.WinUi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Windows.System;

namespace MermaYT.WinUi.Views;

public sealed partial class DownloadsPage :
    Page,
    INotifyPropertyChanged
{
    private readonly IYouTubeDownloadAdapter _youTubeDownloadAdapter;

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

    public ObservableCollection<DownloadItemModel> DownloadQueue { get; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    public DownloadsPage()
    {
        _youTubeDownloadAdapter = ((App)Application.Current)
            .Services
            .GetRequiredService<IYouTubeDownloadAdapter>();

        InitializeComponent();

        CheckAddButtonIsEnabled();
    }

    private void YouTubeUrlTextBox_KeyDown(
        object sender,
        KeyRoutedEventArgs e)
    {
        var enterKeyPressed = e.Key == VirtualKey.Enter &&
            !e.KeyStatus.WasKeyDown;

        if (enterKeyPressed &&
            CanAddToDownloadQueue())
        {
            AddToDownloadQueue();
        }
    }

    private async void BrowseButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var appWindow = ((App)Application.Current)
            .Window?
            .AppWindow;

        if (appWindow is null)
        {
            return;
        }

        var destinationFolderExists = !string.IsNullOrEmpty(DestinationFolder) &&
            Directory.Exists(DestinationFolder);

        var suggestedFolder = destinationFolderExists
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
            DestinationFolder = result.Path;
        }
    }

    private void AddButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        AddToDownloadQueue();
    }

    private void ClearCompletedButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        foreach (var item in DownloadQueue)
        {
            if (item.DownloadState == DownloadState.Completed)
            {
                DownloadQueue.Remove(item);
            }
        }
    }

    private async void DownloadListItem_OpenDestinationFolderButtonClick(
       object sender,
       RoutedEventArgs e)
    {
        if (sender is not DownloadListItem downloadListItem ||
            downloadListItem.Item is null)
        {
            return;
        }

        var destinationFolder = downloadListItem.Item.DestinationFolder;

        var destinationFolderExists = !string.IsNullOrEmpty(destinationFolder) &&
           Directory.Exists(destinationFolder);

        if (destinationFolderExists)
        {
            await Launcher.LaunchFolderPathAsync(destinationFolder);
        }
    }

    private void DownloadListItem_PauseButtonClick(
       object sender,
       RoutedEventArgs e)
    {
        if (sender is not DownloadListItem downloadListItem ||
            downloadListItem.Item is null)
        {
            return;
        }
    }

    private void DownloadListItem_ResumeButtonClick(
       object sender,
       RoutedEventArgs e)
    {
        if (sender is not DownloadListItem downloadListItem ||
            downloadListItem.Item is null)
        {
            return;
        }
    }

    private void DownloadListItem_RemoveButtonClick(
        object sender,
        RoutedEventArgs e)
    {
        if (sender is not DownloadListItem downloadListItem ||
            downloadListItem.Item is null)
        {
            return;
        }

        RemoveFromDownloadQueue(downloadListItem.Item);
    }

    private bool CanAddToDownloadQueue()
    {
        if (string.IsNullOrEmpty(YouTubeUrl))
        {
            return false;
        }

        if (string.IsNullOrEmpty(DestinationFolder) ||
            !Directory.Exists(DestinationFolder))
        {
            return false;
        }

        return true;
    }

    private void CheckAddButtonIsEnabled()
    {
        var isEnabled = CanAddToDownloadQueue();

        if (AddButtonIsEnabled != isEnabled)
        {
            AddButtonIsEnabled = isEnabled;
        }
    }

    private void AddToDownloadQueue()
    {
        try
        {
            var downloadItem = new DownloadItemModel()
            {
                Url = YouTubeUrl,
                Title = YouTubeUrl,
                OutputFormat = SelectedOutputFormat,
                DestinationFolder = DestinationFolder
            };

            DownloadQueue.Add(downloadItem);

            YouTubeUrl = string.Empty;

            var processId = _youTubeDownloadAdapter.Download(
               downloadItem.Url,
               downloadItem.OutputFormat,
               downloadItem.DestinationFolder);

            downloadItem.ProcessId = processId;
            downloadItem.DownloadState = DownloadState.Processing;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(
                $"Error while adding to download queue: {ex.Message}");

            ShowError("Failed to add the download. Please check the URL and try again.");
        }
    }

    private void RemoveFromDownloadQueue(
        DownloadItemModel item)
    {
        try
        {
            if (item.ProcessId != -1)
            {
                _youTubeDownloadAdapter.CancelDownload(
                    item.ProcessId);
            }

            DownloadQueue.Remove(item);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(
                $"Error while removing from download queue: {ex.Message}");

            ShowError("Failed to remove the download. Please try again.");
        }
    }

    private void ShowError(
        string message)
    {
        ErrorInfoBar.Message = message;
        ErrorInfoBar.IsOpen = true;
    }

    private void NotifyPropertyChanged(
        [CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
