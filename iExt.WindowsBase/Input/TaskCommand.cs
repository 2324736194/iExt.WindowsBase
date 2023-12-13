using System.ComponentModel;
using System.Events;

namespace System.Windows.Input
{
    /// <summary>
    /// 任务命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class TaskCommand<T>
    {
        private readonly TaskCommandExecuteHandler<T> _execute;
        private readonly TaskCommandCanExecuteHandler<T> _canExecute;
        private readonly IWeakEventRelay _relayCanExecuteChanged;
        private readonly IWeakEventRelay _relayPropertyChanged;
        private bool _can;
        private bool _executing;
        
        /// <summary>
        /// 命令是否正在执行
        /// </summary>
        public bool IsExecuting
        {
            get => _executing;

            private set => this.SetValue(ref _executing, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute">执行委托</param>
        /// <param name="canExecute">是否执行委托</param>
        public TaskCommand(TaskCommandExecuteHandler<T> execute, TaskCommandCanExecuteHandler<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            _relayCanExecuteChanged = this.GetRelayCanExecuteChanged();
            _relayPropertyChanged = this.GetRelayPropertyChanged();
        }

        private async void CanExecute(T parameter)
        {
            var result = await _canExecute.Invoke(parameter);
            OnCanChanged(result);
        }

        private void OnCanChanged(bool can)
        {
            if (_can == can)
            {
                return;
            }

            _can = can;
            this.RaiseCanExecuteChanged();
        }
    }

    partial class TaskCommand<T> : IRaisePropertyChanged
    {
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => _relayPropertyChanged.Add(value);
            remove => _relayPropertyChanged.Remove(value);
        }

        void IRaisePropertyChanged.RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            _relayPropertyChanged.Raise(this, e);
        }
    }

    partial class TaskCommand<T> : IRaiseCanExecuteChanged
    {
        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            var result = false;
            if (null != _canExecute)
            {
                result = false;
                var commandParameter = this.GetCommandParameter<T>(parameter);
                CanExecute(commandParameter);
            }
            else
            {
                result = true;
            }
            OnCanChanged(result);
            return _can;
        }

        /// <inheritdoc />
        public async void Execute(object parameter)
        {
            try
            {
                OnCanChanged(false);
                IsExecuting = true;
                var commandParameter = this.GetCommandParameter<T>(parameter);
                await _execute.Invoke(commandParameter);
            }
            finally
            {
                IsExecuting = false;
                OnCanChanged(true);
            }
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add => _relayCanExecuteChanged.Add(value);
            remove => _relayCanExecuteChanged.Remove(value);
        }

        void IRaiseCanExecuteChanged.RaiseCanExecuteChanged()
        {
            _relayCanExecuteChanged.Raise(this, EventArgs.Empty);
        }
    }
}