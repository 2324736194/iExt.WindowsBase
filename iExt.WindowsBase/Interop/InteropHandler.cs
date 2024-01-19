using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.HeaderFiles.WinGdi;
using System.Runtime.InteropServices.HeaderFiles.Winuser;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace System.Windows.Interop
{
    /// <summary>
    /// 交互操作处理
    /// </summary>
    public sealed class InteropHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="structure"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IntPtr ToPtr<T>(T structure)
            where T : struct
        {
            var cb = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(cb);
            Marshal.StructureToPtr(structure, ptr, true);
            return ptr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IntPtr ToPtr(byte[] source)
        {
            var length = source.Length;
            var destination = Marshal.AllocHGlobal(length);
            Marshal.Copy(source, 0, destination, length);
            return destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IntPtr ToIcon(ImageSource source)
        {
            var iconSize = new Size(SystemParameters.IconWidth, SystemParameters.IconHeight);
            var colors = CreateBitmap(source, iconSize);
            var width = (int)iconSize.Width;
            var height = (int)iconSize.Height;
            var xHotspot = 0;
            var yHotspot = 0;
            return CreateIconOrCursor(colors, width, height, xHotspot, yHotspot, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IntPtr ToCursor(ImageSource source)
        {
            var cursorSize = new Size(SystemParameters.CursorWidth, SystemParameters.CursorHeight);
            var colors = CreateBitmap(source, cursorSize);
            var width = (int)cursorSize.Width;
            var height = (int)cursorSize.Height;
            var xHotspot = 0;
            var yHotspot = height;
            return CreateIconOrCursor(colors, width, height, xHotspot, yHotspot, false);
        }

        private byte[] CreateBitmap(ImageSource source, Size size)
        {
            var asGoodAsItGets = false;
            var bitmapFrame = source as BitmapFrame;

            if (bitmapFrame?.Decoder?.Frames != null)
            {
                bitmapFrame = GetBestMath(bitmapFrame.Decoder.Frames, size);
                if (bitmapFrame.Decoder is IconBitmapDecoder)
                {
                    asGoodAsItGets = true;
                }
                else if (bitmapFrame.PixelWidth == (int)size.Width && bitmapFrame.PixelHeight == (int)size.Height)
                {
                    asGoodAsItGets = true;
                }

                source = bitmapFrame;
            }

            if (!asGoodAsItGets)
            {
                var bitmapSource = CreateBitmapSource(source, size);
                bitmapFrame = BitmapFrame.Create(bitmapSource);
            }

            return CreateBitmap(bitmapFrame);
        }

        private byte[] CreateBitmap(BitmapFrame bitmapFrame)
        {
            var bitmapSource = (BitmapSource)bitmapFrame;

            if (bitmapSource.Format != PixelFormats.Bgra32 && bitmapSource.Format != PixelFormats.Pbgra32)
            {
                bitmapSource = new FormatConvertedBitmap(bitmapSource, PixelFormats.Bgra32, null, 0.0);
            }

            var w = bitmapSource.PixelWidth;
            var h = bitmapSource.PixelHeight;
            var bpp = bitmapSource.Format.BitsPerPixel;
            var stride = (bpp * w + 31) / 32 * 4;
            var sizeCopyPixels = stride * h;
            var pixels = new byte[sizeCopyPixels];
            bitmapSource.CopyPixels(pixels, stride, 0);
            return pixels;
        }

        private IntPtr CreateIconOrCursor(byte[] colors, int width, int height, int xHotspot, int yHotspot, bool isIcon)
        {
            var colorBitmap = default(IntPtr);
            var maskBitmap = default(IntPtr);
            try
            {
                var header = new BITMAPINFOHEADER(width, -height, 32);
                var bitmapinfo = new BITMAPINFO()
                {
                    bmiHeader = header
                };
                var pbmi = ToPtr(bitmapinfo);
                var bits = default(IntPtr);
                colorBitmap = Gdi32.CreateDIBSection(IntPtr.Zero, pbmi, DIB.RGB_COLORS, ref bits, default, 0);

                if (colorBitmap == IntPtr.Zero || bits == IntPtr.Zero)
                {
                    throw new Exception("创建一个可写入的 DIB 应用程序失败。");
                }

                Marshal.Copy(colors, 0, bits, colors.Length);
                var maskArray = CreateMaskArray(width, height, colors);
                var lpBits = ToPtr(maskArray);
                maskBitmap = Gdi32.CreateBitmap(width, height, 1, 1, lpBits);
                if (IntPtr.Zero == maskBitmap)
                {
                    throw new Exception("创建一个可写入的 DIB 应用程序失败。");
                }

                var iconinfo = new ICONINFO
                {
                    fIcon = isIcon,
                    xHotspot = (uint)xHotspot,
                    yHotspot = (uint)yHotspot,
                    hbmMask = maskBitmap,
                    hbmColor = colorBitmap
                };
                var piconinfo = ToPtr(iconinfo);
                var handle = User32.CreateIconIndirect(piconinfo);
                return handle;
            }
            catch (Exception ex)
            {
                throw new Exception("创建互操作位图失败", ex);
            }
            finally
            {
                Gdi32.DeleteObject(colorBitmap);
                Gdi32.DeleteObject(maskBitmap);
            }
        }

        private byte[] CreateMaskArray(int width, int height, byte[] colorArray)
        {
            var nCount = width * height;
            var bytesPerScanLine = AlignToBytes(width, 2) / 8;
            var bitsMask = new byte[bytesPerScanLine * height];

            for (var i = 0; i < nCount; i++)
            {
                var hPos = i % width;
                var vPos = i / width;
                var byteIndex = hPos / 8;
                var offsetBit = (byte)(0x80 >> (hPos % 8));

                if (colorArray[i * 4 + 3] == 0x00)
                {
                    bitsMask[byteIndex + bytesPerScanLine * vPos] |= offsetBit;
                }
                else
                {
                    bitsMask[byteIndex + bytesPerScanLine * vPos] &= (byte)~offsetBit;
                }

                if (hPos == width - 1 && width == 8)
                {
                    bitsMask[1 + bytesPerScanLine * vPos] = 0xff;
                }
            }

            return bitsMask;
        }

        private int AlignToBytes(double original, int nBytesCount)
        {
            var nBitsCount = 8 << (nBytesCount - 1);
            return ((int)Math.Ceiling(original) + (nBitsCount - 1)) / nBitsCount * nBitsCount;
        }

        /// <summary>
        /// 创建位图源
        /// </summary>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private BitmapSource CreateBitmapSource(ImageSource source, Size size)
        {
            var drawingDimensions = new Rect(0, 0, size.Width, size.Height);

            var renderRatio = size.Width / size.Height;
            var aspectRatio = source.Width / source.Height;

            if (source.Width <= size.Width && source.Height <= size.Height)
            {
                drawingDimensions = new Rect((size.Width - source.Width) / 2, (size.Height - source.Height) / 2, source.Width, source.Height);
            }
            else if (renderRatio > aspectRatio)
            {
                var scaledRenderWidth = (source.Width / source.Height) * size.Width;
                drawingDimensions = new Rect((size.Width - scaledRenderWidth) / 2, 0, scaledRenderWidth, size.Height);
            }
            else if (renderRatio < aspectRatio)
            {
                var scaledRenderHeight = source.Height / source.Width * size.Height;
                drawingDimensions = new Rect(0, (size.Height - scaledRenderHeight) / 2, size.Width, scaledRenderHeight);
            }

            var dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            dc.DrawImage(source, drawingDimensions);
            dc.Close();

            var bmp = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);
            return bmp;
        }

        private BitmapFrame GetBestMath(IReadOnlyList<BitmapFrame> source, Size size)
        {
            var bestScore = int.MaxValue;
            var bestBpp = 0;
            var bestIndex = 0;
            var isBitmapIconDecoder = source[0].Decoder is IconBitmapDecoder;

            for (var i = 0; i < source.Count && bestScore != 0; ++i)
            {
                var currentIconBitDepth = isBitmapIconDecoder ? source[i].Thumbnail.Format.BitsPerPixel : source[i].Format.BitsPerPixel;

                if (currentIconBitDepth == 0)
                {
                    currentIconBitDepth = 8;
                }

                var score = MatchImage(source[i], size, currentIconBitDepth);
                if (score < bestScore)
                {
                    bestIndex = i;
                    bestBpp = currentIconBitDepth;
                    bestScore = score;
                }
                else if (score == bestScore)
                {
                    if (bestBpp < currentIconBitDepth)
                    {
                        bestIndex = i;
                        bestBpp = currentIconBitDepth;
                    }
                }
            }

            return source[bestIndex];
        }

        private int MatchImage(BitmapFrame frame, Size size, int bpp)
        {
            var desktop = User32.GetDC(IntPtr.Zero);
            var bitSpixel = Gdi32.GetDeviceCaps(desktop, DCP.BITSPIXEL);
            var planes = Gdi32.GetDeviceCaps(desktop, DCP.PLANES);
            var bitDepth = bitSpixel * planes;
            if (bitDepth == 8)
            {
                bitDepth = 4;
            }

            var score = 2 * Abs(bpp, bitDepth, false) +
                        Abs(frame.PixelWidth, (int)size.Width, true) +
                        Abs(frame.PixelHeight, (int)size.Height, true);

            return score;
        }

        private int Abs(int valueHave, int valueWant, bool fPunish)
        {
            var diff = (valueHave - valueWant);

            if (diff < 0)
            {
                diff = (fPunish ? -2 : -1) * diff;
            }

            return diff;
        }
    }
}
