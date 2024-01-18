using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace System.Windows
{
    /// <summary>
    /// 元素树遍历处理者
    /// </summary>
    public sealed class TreeForeachHandler
    {
        /// <summary>
        /// 元素树状结构
        /// </summary>
        public TreeForeachCategory ForeachCategory { get; set; }

        /// <summary>
        /// 遍历模式
        /// </summary>
        public TreeForeachMode ForeachMode { get; set; }

        /// <summary>
        /// 遍历标识处理
        /// </summary>
        public TreeForeachFlagHandler FlagHandler { get; set; }

        /// <summary>
        /// 遍历元素
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Handle(DependencyObject source)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            try
            {
                ForeachHandler(0, source);
            }
            catch (TaskCanceledException)
            {

            }
        }

        private void ForeachHandler(int index, object o)
        {
            var flag = FlagHandler?.Invoke(index, o) ?? ForeachFlag.Normal;
            switch (flag)
            {
                case ForeachFlag.Normal:
                    break;
                case ForeachFlag.Break:
                    throw new TaskCanceledException();
                case ForeachFlag.Continue:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (o is DependencyObject dependencyObject)
            {
                DependencyObjectHandler(index, dependencyObject);
            }
        }

        private void DependencyObjectHandler(int index, DependencyObject dependencyObject)
        {
            index++;
            switch (ForeachCategory)
            {
                case TreeForeachCategory.LogicalTree:
                    LogicalTreeHandler(index, dependencyObject);
                    break;
                case TreeForeachCategory.VisualTree:
                    VisualTreeHandler(index, dependencyObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void VisualTreeHandler(int index, DependencyObject dependencyObject)
        {
            switch (ForeachMode)
            {
                case TreeForeachMode.Children:
                    var childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
                    for (var i = 0; i < childrenCount; i++)
                    {
                        var child = VisualTreeHelper.GetChild(dependencyObject, i);
                       ForeachHandler(index, child);
                    }
                    break;
                case TreeForeachMode.Parent:
                    var parent = VisualTreeHelper.GetParent(dependencyObject);
                    if (null != parent)
                    {
                        ForeachHandler(index, parent);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LogicalTreeHandler(int index, DependencyObject dependencyObject)
        {
            switch (ForeachMode)
            {
                case TreeForeachMode.Children:
                    var children = LogicalTreeHelper.GetChildren(dependencyObject);
                    if (null != children)
                    {
                        foreach (var child in children)
                        {
                            ForeachHandler(index, child);
                        }
                    }
                    break;
                case TreeForeachMode.Parent:
                    var parent = LogicalTreeHelper.GetParent(dependencyObject);
                    if (null != parent)
                    {
                        ForeachHandler(index, parent);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <summary>
    /// 元素树遍历标识处理者
    /// </summary>
    /// <param name="index">层级编号</param>
    /// <param name="element">当前元素</param>
    /// <returns></returns>
    public delegate ForeachFlag TreeForeachFlagHandler(int index, object element);
}