using MermaYT.Core.YouTubeDownloader;
using MermaYT.Core.YouTubeDownloader.Abstractions;
using MermaYT.Core.YouTubeDownloader.Events;
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.System;

namespace MermaYT.WinUi.Views;

public sealed partial class DownloadsPage :
    Page,
    INotifyPropertyChanged
{
    private readonly IYouTubeDownloadManager _youTubeDownloadAdapter;

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
            .GetRequiredService<IYouTubeDownloadManager>();

        _youTubeDownloadAdapter.DownloadErrorReceived += OnDownloadErrorReceived;

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
        await SelectDestinationFolder();
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
        ClearCompletedDownloads();
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

        await OpenDestinationFolder(
            downloadListItem.Item.DestinationFolder);
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

    private void OnDownloadErrorReceived(
        object? sender,
        DownloadErrorEventArgs e)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            var downloadItem = DownloadQueue.FirstOrDefault(
                item => item.ProcessId == e.DownloadItemId);

            if (downloadItem is null)
            {
                return;
            }

            UpdateErrorState(
                downloadItem,
                e.ErrorMessage);
        });
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

    private async Task SelectDestinationFolder()
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

    private async Task OpenDestinationFolder(
        string destinationFolder)
    {
        var destinationFolderExists = !string.IsNullOrEmpty(destinationFolder) &&
           Directory.Exists(destinationFolder);

        if (destinationFolderExists)
        {
            await Launcher.LaunchFolderPathAsync(destinationFolder);
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

            var processId = _youTubeDownloadAdapter.Download(
                downloadItem.Url,
                downloadItem.OutputFormat,
                downloadItem.DestinationFolder);

            downloadItem.ProcessId = processId;
            downloadItem.DownloadState = DownloadState.Processing;

            YouTubeUrl = string.Empty;

            DownloadQueue.Add(downloadItem);
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
                _youTubeDownloadAdapter.Cancel(
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

    private void UpdateErrorState(
        DownloadItemModel downloadItem,
        string errorMessage)
    {
        if (downloadItem.DownloadState == DownloadState.Error ||
            downloadItem.DownloadState == DownloadState.Completed)
        {
            return;
        }

        downloadItem.DownloadState = DownloadState.Error;
        downloadItem.ErrorMessage = errorMessage;
    }

    private void ClearCompletedDownloads()
    {
        foreach (var item in DownloadQueue)
        {
            if (item.DownloadState == DownloadState.Completed)
            {
                DownloadQueue.Remove(item);
            }
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
