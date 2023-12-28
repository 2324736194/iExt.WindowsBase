using System;
using System.Collections.Generic;
using System.Events;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Input
{
    /// <summary>
    /// 针对 <see cref="ICommand"/> 的扩展
    /// </summary>
    public static class CommandExt
    {
        /// <summary>
        /// 触发 <see cref="ICommand.CanExecuteChanged"/> 事件
        /// </summary>
        /// <param name="source"></param>
        public static void RaiseCanExecuteChanged(this ICommand source)
        {
            if (source is IRaiseCanExecuteChanged raise)
            {
                raise.RaiseCanExecuteChanged();
                return;
            }

            throw new NotImplementedException($"当前命令未继承接口 {nameof(IRaiseCanExecuteChanged)}");
        }

        /// <summary>
        /// 获取隐式事件 <see cref="ICommand.CanExecuteChanged"/> 对应的弱事件
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IWeakEventRelay GetRelayCanExecuteChanged(this ICommand source)
        {
            var interfaceName = typeof(ICommand).FullName;
            var eventName = nameof(ICommand.CanExecuteChanged);
            var name = $"{interfaceName}.{eventName}";
            return source.RegisterWeakEvent(name);
        }

        internal static T GetCommandParameter<T>(this ICommand source, object parameter)
        {
            var genericTypeInfo = typeof(T).GetTypeInfo();
            
            if (genericTypeInfo.IsValueType)
            {
                if ((!genericTypeInfo.IsGenericType) || (!typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition().GetTypeInfo())))
                {
                    throw new Exception("当前命令参数类型无法转换成泛型类型");
                }
            }
            
            return (T)parameter;
        }
    }
}
