using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Prism.Commands;

namespace iExt.WindowsBase.Demo.ViewModels.Media.Imaging
{
    internal class ImageSourceHandlerViewModel
    {
        public ICommand SaveCommand { get; }

        public ImageSourceHandlerViewModel()
        {
            SaveCommand = new DelegateCommand<Image>(SaveCommandExecuteMethod);
        }

        private void SaveCommandExecuteMethod(Image image)
        {
            var handler = new ImageSourceHandler();
            var stream = handler.ToStream(image.Source);
            var di = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var name = $"{Guid.NewGuid():N}.png";
            var fileName = Path.Combine(di, name);
            var file = File.Create(fileName);
            using (file)
            {
                using (stream)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(file);
                }
            }

            MessageBox.Show($"文件已保存到：{fileName}");
        }
    }
}
