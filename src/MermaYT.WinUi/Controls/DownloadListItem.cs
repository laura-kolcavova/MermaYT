using MermaYT.WinUi.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace MermaYT.WinUi.Controls;

public sealed partial class DownloadListItem :
    Control
{
    private Button? _openDestinationFolderButton;

    private Button? _pauseButton;

    private Button? _resumeButton;

    private Button? _removeButton;

    public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
        nameof(Item),
        typeof(DownloadItemModel),
        typeof(DownloadListItem),
        new PropertyMetadata(null, OnItemChanged));

    public DownloadItemModel? Item
    {
        get => (DownloadItemModel?)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }


    public event RoutedEventHandler? OpenDestinationFolderButtonClick;

    public event RoutedEventHandler? PauseButtonClick;

    public event RoutedEventHandler? ResumeButtonClick;

    public event RoutedEventHandler? RemoveButtonClick;

    public DownloadListItem()
    {
        DefaultStyleKey = typeof(DownloadListItem);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        InitializeOpenDestinationFolderButton();
        InitializePauseButton();
        InitializeResumeButton();
        InitializeRemoveButton();

        var downloadState = Item?.DownloadState
           ?? DownloadState.Queued;

        GoToDownloadState(downloadState);
    }

    private void InitializeOpenDestinationFolderButton()
    {
        if (_openDestinationFolderButton is not null)
        {
            _openDestinationFolderButton.Click -= OnOpenDestinationFolderButtonClick;
        }

        _openDestinationFolderButton = GetTemplateChild("OpenDestinationFolderButton") as Button;

        if (_openDestinationFolderButton is not null)
        {
            _openDestinationFolderButton.Click += OnOpenDestinationFolderButtonClick;
        }
    }

    private void InitializePauseButton()
    {
        if (_pauseButton is not null)
        {
            _pauseButton.Click -= OnPauseButtonClick;
        }

        _pauseButton = GetTemplateChild("PauseButton") as Button;

        if (_pauseButton is not null)
        {
            _pauseButton.Click += OnPauseButtonClick;
        }
    }

    private void InitializeResumeButton()
    {
        if (_resumeButton is not null)
        {
            _resumeButton.Click -= OnResumeButtonClick;
        }

        _resumeButton = GetTemplateChild("ResumeButton") as Button;

        if (_resumeButton is not null)
        {
            _resumeButton.Click += OnResumeButtonClick;
        }
    }

    private void InitializeRemoveButton()
    {
        if (_removeButton is not null)
        {
            _removeButton.Click -= OnRemoveButtonClick;
        }
        _removeButton = GetTemplateChild("RemoveButton") as Button;

        if (_removeButton is not null)
        {
            _removeButton.Click += OnRemoveButtonClick;
        }
    }

    private void OnOpenDestinationFolderButtonClick(
        object sender,
        RoutedEventArgs e)
    {
        OpenDestinationFolderButtonClick?.Invoke(this, e);
    }

    private void OnPauseButtonClick(
        object sender,
        RoutedEventArgs e)
    {
        PauseButtonClick?.Invoke(this, e);
    }

    private void OnResumeButtonClick(
        object sender,
        RoutedEventArgs e)
    {
        ResumeButtonClick?.Invoke(this, e);
    }

    private void OnRemoveButtonClick(
        object sender,
        RoutedEventArgs e)
    {
        RemoveButtonClick?.Invoke(this, e);
    }

    private static void OnItemChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (DownloadListItem)d;

        if (e.OldValue is DownloadItemModel old)
        {
            old.PropertyChanged -= self.OnItemPropertyChanged;
        }

        if (e.NewValue is DownloadItemModel next)
        {
            next.PropertyChanged += self.OnItemPropertyChanged;
        }

        var downloadState = (e.NewValue as DownloadItemModel)?.DownloadState
            ?? DownloadState.Queued;

        self.GoToDownloadState(downloadState);
    }

    private void OnItemPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DownloadItemModel.DownloadState))
        {
            GoToDownloadState(Item?.DownloadState ?? DownloadState.Queued);
        }
    }

    private void GoToDownloadState(DownloadState state)
    {
        VisualStateManager.GoToState(this, state.ToString(), useTransitions: true);
    }
}
