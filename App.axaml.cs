using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DisneylandMap.src.Graphe;

namespace DisneylandMap;

public partial class App : Application
{
    public static MainWindow? MainWindow { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Graphe.Initialiser(); // Initilisation des données du graphe

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            App.MainWindow = desktop.MainWindow as MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}