using ClickMapper.View.MainWindow.Pages;

namespace ClickMapper.View.MainWindow
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() { NavigationService.Navigate(new MainPage()); }
    }
}