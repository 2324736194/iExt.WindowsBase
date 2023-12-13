using System;
using System.Diagnostics;
using System.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace iExt.WindowsBase.Demo.Views
{
    /// <summary>
    /// ScreenshotView.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenshotView : UserControl
    {
        private Window _displayer;

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
                    var relay = main.RegisterWeakEvent(nameof(Window.Activated), RegisterActivated);
                    var handler = new EventHandler(DisplayerActivatedHandler);
                    relay.Add(handler);
                    var w = new Window();
                    _displayer = w;
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

        private void RegisterActivated(Window owner, IWeakEventRelay relay)
        {
            owner.Activated += (sender, e) =>
            {
                relay.Raise(sender, e);
            };
        }

        private void DisplayerActivatedHandler(object sender, EventArgs e)
        {
            Debug.WriteLine(nameof(DisplayerActivatedHandler));
            if (null == _displayer) return;
            _displayer.Activate();
        }

        private void ClickCloseHandler(object sender, MouseButtonEventArgs e)
        {
            if (sender is Window window)
            {
                _displayer = null;
                window.Close();
            }
        }
    }
}
