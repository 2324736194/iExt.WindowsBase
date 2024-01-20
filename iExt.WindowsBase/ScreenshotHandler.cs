using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace System.Windows
{
    /// <summary>
    /// 截图处理
    /// </summary>
    public sealed class ScreenshotHandler
    {
        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ImageSource Handle(FrameworkElement element)
        {
            if (null == element)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (double.IsNaN(element.ActualHeight) ||
                double.IsNaN(element.ActualWidth) ||
                element.ActualWidth < 1 ||
                element.ActualHeight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(element));
            }
            var format = PixelFormats.Default;
            var dpiScale = new DpiScale(element);
            var width = (int)element.ActualWidth;
            var height = (int)element.ActualHeight;
            var source = new RenderTargetBitmap(width, height, dpiScale.PixelsPerInchX, dpiScale.PixelsPerInchY, format);
            source.Render(element);
            return source;
        }
    }
}