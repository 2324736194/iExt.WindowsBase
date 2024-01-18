using System.Diagnostics;
using System.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace iExt.WindowsBase.Demo.ViewModels
{
    internal class ScreenshotHandlerViewModel : BindableBase
    {
        public ICommand ScreenshotCommand { get; }

        public ScreenshotHandlerViewModel()
        {
            ScreenshotCommand = new DelegateCommand<FrameworkElement>(ScreenshotCommandExecuteMethod);
        }

        private void ScreenshotCommandExecuteMethod(FrameworkElement element)   
        {
            var screenshotHandler = new ScreenshotHandler();
            var source = screenshotHandler.Handle(element);
            var image = new Image();
            image.Width = element.ActualWidth;
            image.Height = element.ActualHeight;
            image.Source = source;
            var main = Application.Current.MainWindow;
            var window = new Window();
            window.Owner = main;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = image;
            window.Padding = new Thickness(10);
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.ResizeMode = ResizeMode.NoResize;
            window.ShowDialog();
        }
        
        private void ClickCloseHandler(object sender, MouseButtonEventArgs e)
        {
            if (sender is Window window)
            {
                window.Close();
            }
        }
    }
}