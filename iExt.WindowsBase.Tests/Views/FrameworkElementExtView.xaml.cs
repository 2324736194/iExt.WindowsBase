using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace iExt.WindowsBase.Tests.Views
{
    /// <summary>
    /// FrameworkElementExtView.xaml 的交互逻辑
    /// </summary>
    public partial class FrameworkElementExtView : UserControl
    {
        public FrameworkElementExtView()
        {
            InitializeComponent();
        }

        private void Screenshot_OnClick(object sender, RoutedEventArgs e)
        {
            var png = ".png";   
            var dialog = new SaveFileDialog();
            dialog.Title = "截图保存";
            dialog.Filter = $"图片|{png}";
            dialog.FileOk+= OnSaveFileOk;
            dialog.FileName = $"{Guid.NewGuid():N}{png}";
            dialog.DefaultExt = png;
            dialog.ShowDialog();
        }

        private void OnSaveFileOk(object sender, CancelEventArgs e)
        {
            var dialog = (SaveFileDialog)sender;
            var img = ScreenshotPanel.Screenshot();
        }
    }
}
