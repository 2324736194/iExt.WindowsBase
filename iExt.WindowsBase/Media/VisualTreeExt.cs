using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Media
{
    /// <summary>
    /// 视觉树扩展
    /// </summary>
    public static class VisualTreeExt
    {
        /// <summary>
        /// 遍历视觉树
        /// </summary>
        /// <param name="source"></param>
        /// <param name="handler"></param>
        /// <param name="downward"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ForeachVisualTree<T>(this DependencyObject source, Func<T, ForeachFlag> handler, bool downward = true)
        {
            if (null == source)
            {   
                throw new ArgumentNullException(nameof(source));
            }
            if (null == handler)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (null == handler)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var searchHandler = default(Action<List<object>, DependencyObject>);
            if (downward)
            {
                searchHandler = SearchChildrenHandler;
            }
            else
            {
                searchHandler = SearchParentHandler;
            }

            LogicalTreeExt.Foreach(source, handler, searchHandler);
        }

        static void SearchParentHandler(List<object> list, DependencyObject o)
        {
            var parent = VisualTreeHelper.GetParent(o);
            if (null == parent)
            {
                return;
            }

            list.Add(parent);
        }

        static void SearchChildrenHandler(List<object> list, DependencyObject o)
        {
            var count = VisualTreeHelper.GetChildrenCount(o);
            var children = new List<object>();
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);
                children.Add(child);
            }
            var index = list.IndexOf(o);
            list.InsertRange(index, children);
        }
    }
}
