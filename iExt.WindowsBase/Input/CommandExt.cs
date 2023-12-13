using System;
using System.Collections.Generic;
using System.Events;
using System.Linq;
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
        /// <param name="interface"></param>
        public static void RaiseCanExecuteChanged(this IRaiseCanExecuteChanged @interface)
        {
            @interface.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 获取隐式事件 <see cref="ICommand.CanExecuteChanged"/> 对应的弱事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        public static IWeakEventRelay GetRelayCanExecuteChanged(this ICommand source, RegisterEvent<ICommand> register = null)
        {
            var interfaceName = typeof(ICommand).FullName;
            var eventName = nameof(ICommand.CanExecuteChanged);
            var name = $"{interfaceName}.{eventName}";
            return source.RegisterWeakEvent(name, register);
        }

        internal static T GetCommandParameter<T>(this ICommand source, object parameter)
        {
            if (!(parameter is T commandParameter))
            {
                throw new Exception("当前命令参数类型无法转换成泛型类型");
            }

            return commandParameter;
        }
    }
}
