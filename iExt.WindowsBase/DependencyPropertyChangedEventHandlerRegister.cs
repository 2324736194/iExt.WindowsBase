using System.Events;
using System.Reflection;

namespace System.Windows
{
    /// <summary>
    /// 注册委托类型为 <see cref="DependencyPropertyChangedEventHandler"/> 的事件
    /// </summary>
    public sealed class DependencyPropertyChangedEventHandlerRegister : IWeakEventRegister
    {
        private WeakReference<IWeakEventRelay> _relay;

        /// <inheritdoc />
        public MethodInfo GetRegisterMethod(IWeakEventRelay relay)
        {
            if (relay == null)
                throw new ArgumentNullException(nameof(relay));
            var bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
            var method = GetType().GetMethod(nameof(Raise), bindingAttr);
            _relay = new WeakReference<IWeakEventRelay>(relay);
            return method;
        }

        private void Raise(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_relay.TryGetTarget(out var target))
            {
                target.Raise(sender, e);
            }
        }
    }
}