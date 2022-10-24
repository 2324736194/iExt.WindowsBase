using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace System.Windows
{
    /// <summary>
    /// <see cref="FrameworkElement"/> 扩展
    /// </summary>
    public static class FrameworkElementExt
    {
        /// <summary>
        /// 对元素进行截图
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dpi"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static RenderTargetBitmap Screenshot(this FrameworkElement element,
            Dpi dpi, PixelFormat format)
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
            var width = (int)element.ActualWidth;
            var height = (int)element.ActualHeight;
            var source = new RenderTargetBitmap(width, height, dpi.X, dpi.Y, format);
            source.Render(element);
            return source;
        }

        /// <summary>
        /// <para>对元素进行截图</para>
        /// <para>注意：</para>
        /// <para>当前元素的高宽不允许为 0</para>
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static RenderTargetBitmap Screenshot(this FrameworkElement element)
        {
            var dpiScale = Dpi.GetDpiFromVisual(element);
            var format = PixelFormats.Default;
            return Screenshot(element, dpiScale, format);
        }
    }
}
