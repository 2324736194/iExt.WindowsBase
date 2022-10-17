using System.Windows;
using System.Windows.Media.Media3D;

namespace iExt.WindowsBase.Tests.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string ContentRegion { get; }

        static MainWindow()
        {
            var ownerType = typeof(MainWindow);
            ContentRegion = ownerType.Name + nameof(ContentRegion);
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
