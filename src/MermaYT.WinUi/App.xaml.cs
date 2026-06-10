using MermaYT.Core.YouTubeDownloader.Extensions;
using MermaYT.WinUi.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace MermaYT.WinUi;

public partial class App : Application
{
    public IServiceProvider Services { get; }

    public Window? Window { get; private set; }

    public App()
    {
        Services = new ServiceCollection()
           .AddYouTubeDownloader()
           .BuildServiceProvider();

        InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        Window = new MainWindow();
        Window.Activate();
    }
}
