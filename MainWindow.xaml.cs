using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace SmartPixel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
        }
    }
}
