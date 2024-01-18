using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows
{
    /// <summary>
    /// 元素树遍历处理者
    /// </summary>
    public sealed class TreeForeachHandler
    {
        private Queue<object> _queue;

        /// <summary>
        /// 元素树状结构
        /// </summary>
        public TreeCategories Category { get; set; }

        /// <summary>
        /// 遍历模式
        /// </summary>
        public TreeForeachMode ForeachMode { get; set; }

        /// <summary>
        /// 遍历标识处理
        /// </summary>
        public Func<object, ForeachFlag> FlagHandler { get; set; }

        public void Handle(DependencyObject source)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _queue = new Queue<object>();
            _queue.Enqueue(source);
            ForeachHandler();
        }

        private void ForeachHandler()
        {
            do
            {
                var item = _queue.Dequeue();
                var flag = FlagHandler?.Invoke(item) ?? ForeachFlag.Normal;
                switch (flag)
                {
                    case ForeachFlag.Normal:
                        break;
                    case ForeachFlag.Break:
                        return;
                    case ForeachFlag.Continue:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (item is DependencyObject dependencyObject)
                {
                    DependencyObjectHandler(dependencyObject);
                }
            } while (_queue.Count > 0);
        }

        private void DependencyObjectHandler(DependencyObject dependencyObject)
        {
            switch (Category)
            {
                case TreeCategories.LogicalTree:
                    LogicalTreeHandler(dependencyObject);
                    break;
                case TreeCategories.VisualTree:
                    VisualTreeHandler(dependencyObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void VisualTreeHandler(DependencyObject dependencyObject)
        {
            switch (ForeachMode)
            {
                case TreeForeachMode.Children:
                    var parent = VisualTreeHelper.GetParent(dependencyObject);
                    if (null == parent)
                    {
                        _queue.Enqueue(parent);
                    }
                    break;      
                case TreeForeachMode.Parent:
                    var childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
                    for (var i = 0; i < childrenCount; i++)
                    {
                        var child = VisualTreeHelper.GetChild(dependencyObject, i);
                        _queue.Enqueue(child);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LogicalTreeHandler(DependencyObject dependencyObject)
        {
            switch (ForeachMode)
            {
                case TreeForeachMode.Children:
                    var parent = LogicalTreeHelper.GetParent(dependencyObject);
                    if (null == parent)
                    {
                        _queue.Enqueue(parent);
                    }
                    break;
                case TreeForeachMode.Parent:
                    var children = LogicalTreeHelper.GetChildren(dependencyObject);
                    if (null != children)
                    {
                        foreach (var child in children)
                        {
                            _queue.Enqueue(child);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}