﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace System.Windows
{
    /// <summary>
    /// 逻辑树扩展
    /// </summary>
    public static class LogicalTreeExt
    {
        /// <summary>
        /// 遍历逻辑树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">逻辑起点</param>
        /// <param name="handler">
        /// <para>处理逻辑</para>
        /// <para>返回 true 时，立即停止遍历。</para>
        /// </param>
        /// <param name="downward">遍历方向，默认向下遍历</param>
        public static void ForeachLogicalTree<T>(this DependencyObject source, Func<T, ForeachFlag> handler,
            bool downward = true)
        {
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

            Foreach(source, handler, searchHandler);
        }

       internal static void Foreach<T>(DependencyObject source, Func<T, ForeachFlag> handler, Action<List<object>, DependencyObject> dependencyObjectHandler)
        {
            var list = new List<object>();
            list.Add(source);
            while (true)
            {
                if (list.Count <= 0)
                {
                    break;
                }
                var item = list.First();
                if (item is T obj)
                {
                    var breakFlag = false;
                    switch (handler.Invoke(obj))
                    {
                        case ForeachFlag.Normal:
                            break;
                        case ForeachFlag.Break:
                            breakFlag = true;
                            break; ;
                        case ForeachFlag.Continue:
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (breakFlag)
                    {
                        break;
                    }
                }

                if (item is DependencyObject o)
                {
                    dependencyObjectHandler(list, o);
                }

                list.Remove(item);
            }
        }
        
        static void SearchParentHandler(List<object> list, DependencyObject o)
        {
            var parent = LogicalTreeHelper.GetParent(o);
            if (null == parent)
            {
                return;
            }

            list.Add(parent);
        }

        static void SearchChildrenHandler(List<object> list, DependencyObject o)
        {
            var children = LogicalTreeHelper.GetChildren(o).OfType<object>();
            var index = list.IndexOf(o);
            list.InsertRange(index, children);
        }
    }
}