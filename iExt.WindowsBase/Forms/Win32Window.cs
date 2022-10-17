using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace System.Windows.Forms
{
    /// <summary>
    /// <see cref="IWin32Window"/> 接口的 WPF 实现
    /// </summary>
    public class Win32Window : IWin32Window
    {
        private readonly WindowInteropHelper helper;

        /// <inheritdoc />
        public IntPtr Handle => helper.Handle;
        
        /// <summary>
        /// 构造基于 <see cref="DependencyObject"/> 的 <see cref="IWin32Window"/>
        /// </summary>
        /// <param name="dependencyObject">元素</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InstanceNotFoundException"></exception>
        public Win32Window(DependencyObject dependencyObject)
        {
            if (null == dependencyObject)
            {
                throw new ArgumentNullException(nameof(dependencyObject));
            }
            var window = Window.GetWindow(dependencyObject);
            if (null == window)
            {
                throw new InstanceNotFoundException();
            }
            helper = new WindowInteropHelper(window);
        }
    }
}
