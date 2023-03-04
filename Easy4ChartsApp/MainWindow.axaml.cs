using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Microcharts.Example;


namespace Easy4ChartsApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
        this.DataContext = new MainWindowViewModel();
    }
}