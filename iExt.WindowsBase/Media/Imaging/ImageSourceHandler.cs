using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Media.Imaging
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ImageSourceHandler
    {
        /// <summary>
        /// 将 <see cref="ImageSource"/> 转换成 <see cref="Stream"/>
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public Stream ToStream(ImageSource imageSource)
        {
            switch (imageSource)
            {
                case BitmapSource bitmapSource:
                    return ToStream(bitmapSource);
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 将 <see cref="BitmapSource"/> 转换成 <see cref="Stream"/>
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public Stream ToStream(BitmapSource bitmapSource)
        {
            var frame = BitmapFrame.Create(bitmapSource);
            var stream = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);
            encoder.Save(stream);
            stream.Position = 0; // 将流的位置重置为开头
            return stream;
        }
    }
}
