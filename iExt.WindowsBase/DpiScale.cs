using System.Reflection;
using System.Windows.Media;

namespace System.Windows
{
    /// <summary>
    /// 将从中 DPI 信息存储 <see cref="Visual"/> 或 <see cref="UIElement"/> 呈现。
    /// </summary>
    public readonly struct DpiScale
    {
        private readonly double _pixelsPerInchX;
        private readonly double _pixelsPerInchY;

        /// <summary>
        /// 获取 DPI X 轴
        /// </summary>
        public double PixelsPerInchX =>_pixelsPerInchX;

        /// <summary>
        /// 获取沿 Y 轴的 DPI
        /// </summary>
        public double PixelsPerInchY => _pixelsPerInchY;

        /// <summary>
        /// 初始化 <see cref="DpiScale"/> 结构的新实例
        /// </summary>
        /// <param name="visual">视觉对象</param>
        public DpiScale(Visual visual)
        {
            _pixelsPerInchX = 96.0;
            _pixelsPerInchY = 96.0;

            var source = PresentationSource.FromVisual(visual);
            if (source?.CompositionTarget != null)
            {
                _pixelsPerInchX *= source.CompositionTarget.TransformToDevice.M11;
                _pixelsPerInchY *= source.CompositionTarget.TransformToDevice.M22;
            }
        }
    }   
}