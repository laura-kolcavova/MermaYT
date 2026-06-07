using MermaYT.WinUi.Views;
using Microsoft.UI.Xaml;

namespace MermaYT.WinUi;

public partial class App : Application
{
    public Window? Window { get; private set; }

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        Window = new MainWindow();
        Window.Activate();
    }
}
