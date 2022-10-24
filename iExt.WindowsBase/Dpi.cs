using System.Reflection;
using System.Windows.Media;

namespace System.Windows
{
    /// <summary>
    /// 图像每英寸长度内的像素点数
    /// </summary>
    public struct Dpi
    {
        /// <summary>
        /// X 方向
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y 方向
        /// </summary>
        public double Y { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Dpi(double x, double y)
        {
            X = x;
            Y = y;
        }
            
        /// <summary>
        /// 获取 <see cref="Visual"/> 相关的 <see cref="Dpi"/> 数据
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static Dpi GetDpiFromVisual(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);

            var dpiX = 96.0;
            var dpiY = 96.0;

            if (source?.CompositionTarget != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            }

            return new Dpi(dpiX, dpiY);
        }
        
    }   
}