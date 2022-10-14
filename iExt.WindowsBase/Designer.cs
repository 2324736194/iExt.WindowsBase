using System.ComponentModel;

namespace System.Windows
{
    /// <summary>
    /// Xaml 设计器
    /// </summary>
    public sealed class Designer
    {
        /// <summary>
        /// 表示元素是否在 Xaml 设计器中显示
        /// </summary>
        public static bool IsInDesignMode { get; }

        static Designer()
        {
            IsInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
        }
    }
}