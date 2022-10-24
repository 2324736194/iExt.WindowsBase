using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    /// ScreenshotView.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenshotView : UserControl
    {
        private Window displayer;

        public ScreenshotView()
        {
            InitializeComponent();
            Focusable = true;
        }
        
        private void ScreenshotHandler(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                border.ForeachLogicalTree<FrameworkElement>(Handler);
            }
        }

        private ForeachFlag Handler(FrameworkElement element)
        {
            var result = element.Uid == "Screen shot border";
            switch (result)
            {
                case true:
                    Debug.WriteLine(nameof(Handler));
                    var source = element.Screenshot();
                    var image = new Image();
                    image.Width = element.ActualWidth;
                    image.Height = element.ActualHeight;
                    image.Source = source;
                    var main = Application.Current.MainWindow;
                    var events = WeakEventRelay.GetEvents<Window>();
                    var e = events.Single(p => p.Name == nameof(Window.Activated));
                    var relay = main.RegisterWeakEvent(e,RegisterDisplayerActivated);
                    var eHandler = new EventHandler(DisplayerActivatedHandler);
                    relay.Add(eHandler);
                    var w = new Window();
                    displayer = w;
                    w.Owner = main;
                    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    w.Content = image;
                    w.SizeToContent = SizeToContent.WidthAndHeight;
                    w.WindowStyle = WindowStyle.None;
                    w.ResizeMode = ResizeMode.NoResize;
                    w.MouseDown += ClickCloseHandler;
                    w.ShowDialog();
                    return ForeachFlag.Break;
                case false:
                    return ForeachFlag.Normal;
            }
        }

        private void RegisterDisplayerActivated(Window owner, WeakEventRelay relay)
        {
            owner.Activated += (sender, e) =>
            {
                relay.Raise(sender, e);
            };
        }

        private void DisplayerActivatedHandler(object sender, EventArgs e)
        {
            Debug.WriteLine(nameof(DisplayerActivatedHandler));
            if (null == displayer) return;
            displayer.Activate();
        }

        private void ClickCloseHandler(object sender, MouseButtonEventArgs e)
        {
            if (sender is Window window)
            {
                displayer = null;
                window.Close();
            }
        }
    }
}
