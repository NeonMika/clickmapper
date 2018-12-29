using System.Windows;
using ClickMapper.Model;
using JetBrains.Annotations;

namespace ClickMapper.View.MainWindow.Pages
{
    /// <summary>
    ///     Interaction logic for NewMappingPage.xaml
    /// </summary>
    public partial class NewMappingPage
    {
        public NewMappingPage() : this(0, 0) { }

        public NewMappingPage(int x, int y)
        {
            InitializeComponent();
            Mapping = new KeyMapping("", x, y);
            DataContext = Mapping;
        }

        [UsedImplicitly]
        public KeyMapping Mapping { get; set; }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Mapping = null;
            ((Window) Parent).Close();
        }

        private void CreateMappingButton_Click(object sender, RoutedEventArgs e) { ((Window) Parent).Close(); }
    }
}