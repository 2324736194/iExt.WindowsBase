using System.Events;
using System.Reflection;

namespace System.Windows.Input
{
    /// <summary>
    /// 命令
    /// </summary>
    public partial class Command<T>
    {
        private readonly CommandExecuteHandler<T> _execute;
        private readonly CommandCanExecuteHandler<T> _canExecute;
        private readonly IWeakEventRelay _relayCanExecuteChanged;
        private bool _can;

        /// <summary>
        /// 命令
        /// </summary>
        /// <param name="execute">执行委托</param>
        /// <param name="canExecute">是否执行委托</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Command(CommandExecuteHandler<T> execute, CommandCanExecuteHandler<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            var es = GetType().GetEvents(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            _relayCanExecuteChanged = this.GetRelayCanExecuteChanged();
        }
    }

    partial class Command<T> : IRaiseCanExecuteChanged
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add => _relayCanExecuteChanged.Add(value);
            remove => _relayCanExecuteChanged.Remove(value);
        }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            var result = _can;
            if (null != _canExecute)
            {
                var commandParameter = this.GetCommandParameter<T>(parameter);
                result = _canExecute.Invoke(commandParameter);
            }
            else
            {
                result = true;
            }

            if (result != _can)
            {
                _can = result;
                this.RaiseCanExecuteChanged();
            }

            return _can;
        }

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            var commandParameter = this.GetCommandParameter<T>(parameter);
            _execute.Invoke(commandParameter);
        }

        void IRaiseCanExecuteChanged.RaiseCanExecuteChanged()
        {
            _relayCanExecuteChanged.Raise(this, EventArgs.Empty);
        }
    }

}