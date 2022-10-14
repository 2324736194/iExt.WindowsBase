using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace System.Windows.Forms
{
    /// <summary>
    /// <see cref="IWin32Window"/> 接口的实现
    /// </summary>
    public class Win32Window : IWin32Window
    {
        /// <inheritdoc />
        public IntPtr Handle { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <exception cref="Exception"></exception>
        public Win32Window(DependencyObject dependencyObject)
        {
            try
            {
                var window = Window.GetWindow(dependencyObject);
                // ReSharper disable once AssignNullToNotNullAttribute
                var windowInteropHelper = new WindowInteropHelper(window);
                Handle = windowInteropHelper.Handle;
            }
            catch (Exception e)
            {
                throw new Exception("初始化 Win32Window 失败", e);
            }
        }
    }
}
