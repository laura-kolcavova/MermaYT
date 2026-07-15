using MermaYT.Core.YouTubeDownloader;
using MermaYT.WinUi.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MermaYT.WinUi.Controls;

public sealed partial class DownloadListItem :
    Control
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(DownloadListItem),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty OutputFormatProperty = DependencyProperty.Register(
        nameof(OutputFormat),
        typeof(OutputFormat),
        typeof(DownloadListItem),
        new PropertyMetadata(
            OutputFormat.MP3,
            OnOutputFormatChanged));

    public static readonly DependencyProperty OutputFormatTextProperty = DependencyProperty.Register(
        nameof(OutputFormatText),
        typeof(string),
        typeof(DownloadListItem),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
        nameof(Progress),
        typeof(double),
        typeof(DownloadListItem),
        new PropertyMetadata(0));

    public static readonly DependencyProperty ProgressTextProperty = DependencyProperty.Register(
        nameof(ProgressText),
        typeof(string),
        typeof(DownloadListItem),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DownloadedBytesTextProperty = DependencyProperty.Register(
        nameof(DownloadedBytesText),
        typeof(string),
        typeof(DownloadListItem), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TotalBytesTextProperty = DependencyProperty.Register(
        nameof(TotalBytesText),
        typeof(string),
        typeof(DownloadListItem),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DownloadStateProperty = DependencyProperty.Register(
        nameof(DownloadState),
        typeof(DownloadState),
        typeof(DownloadListItem),
        new PropertyMetadata(DownloadState.Processing, OnDownloadStateChanged));

    public static readonly DependencyProperty DownloadStateTextProperty = DependencyProperty.Register(
        nameof(DownloadStateText),
        typeof(string),
        typeof(DownloadListItem),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
        nameof(Item),
        typeof(DownloadItemModel),
        typeof(DownloadListItem),
        new PropertyMetadata(null));

    private Button? _openDestinationFolderButton;
    private Button? _removeButton;

    public event RoutedEventHandler? OpenDestinationFolderButtonClick;

    public event RoutedEventHandler? RemoveButtonClick;

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public OutputFormat OutputFormat
    {
        get => (OutputFormat)GetValue(OutputFormatProperty);
        set => SetValue(OutputFormatProperty, value);
    }

    public string OutputFormatText
    {
        get => (string)GetValue(OutputFormatTextProperty);
        set => SetValue(OutputFormatTextProperty, value);
    }

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public string ProgressText
    {
        get => (string)GetValue(ProgressTextProperty);
        set => SetValue(ProgressTextProperty, value);
    }

    public string DownloadedBytesText
    {
        get => (string)GetValue(DownloadedBytesTextProperty);
        set => SetValue(DownloadedBytesTextProperty, value);
    }

    public string TotalBytesText
    {
        get => (string)GetValue(TotalBytesTextProperty);
        set => SetValue(TotalBytesTextProperty, value);
    }

    public DownloadState DownloadState
    {
        get => (DownloadState)GetValue(DownloadStateProperty);
        set => SetValue(DownloadStateProperty, value);
    }

    public string DownloadStateText
    {
        get => (string)GetValue(DownloadStateTextProperty);
        set => SetValue(DownloadStateTextProperty, value);
    }

    public DownloadItemModel? Item
    {
        get => (DownloadItemModel?)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public DownloadListItem()
    {
        DefaultStyleKey = typeof(DownloadListItem);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        InitializeOpenDestinationFolderButton();
        InitializeRemoveButton();

        GoToDownloadState(DownloadState);
        GoToOutputFormatState(OutputFormat);
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

    private void OnRemoveButtonClick(
        object sender,
        RoutedEventArgs e)
    {
        RemoveButtonClick?.Invoke(this, e);
    }

    private void GoToDownloadState(
        DownloadState downloadState)
    {
        VisualStateManager.GoToState(
            this,
            downloadState.ToString(),
            useTransitions: true);
    }

    private void GoToOutputFormatState(
        OutputFormat outputFormat)
    {
        VisualStateManager.GoToState(
            this,
            outputFormat.ToString(),
            useTransitions: true);
    }

    private static void OnOutputFormatChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (DownloadListItem)d;

        var outputFormat = (OutputFormat)e.NewValue;

        self.GoToOutputFormatState(outputFormat);
    }

    private static void OnDownloadStateChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var self = (DownloadListItem)d;

        var downloadState = (DownloadState)e.NewValue;

        self.GoToDownloadState(downloadState);
    }
}
