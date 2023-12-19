using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Mvvm;

namespace iExt.WindowsBase.Demo.ViewModels.Input
{
    class CommandViewModel : BindableBase
    {
        private readonly int _maxCount;
        private readonly DispatcherTimer _countTimer;
        private bool _counting;
        private int _count;
        private TextBox _displayer;
        
        public ICommand LoadedDisplayerCommand { get; }

        public ICommand CountCommand { get; }

        public ICommand ResetCountCommand { get; }

        public CommandViewModel()
        {
            _maxCount = 30;
            _countTimer = GetCountTimer();
            LoadedDisplayerCommand = new Command<TextBox>(LoadedDisplayerCommandExecute);
            CountCommand = new Command<object>(CountCommandExecute,CountCommandCanExecute);
            ResetCountCommand = new Command<object>(ResetCountCommandExecute);
        }

        private DispatcherTimer GetCountTimer()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick+=CountTimerOnTick;
            return timer;
        }

        private void CountTimerOnTick(object sender, EventArgs e)
        {
            PrintText(_count.ToString());
            _count++;
            if (!CountCommandCanExecute())
            {
                _counting = false;
                CountHandler(_counting);
                CountCommand.RaiseCanExecuteChanged();
            }
        }

        private void LoadedDisplayerCommandExecute(TextBox displayer)
        {
            _displayer = displayer ?? throw new ArgumentNullException(nameof(displayer));
        }

        private void ResetCountCommandExecute(object parameter)
        {
            _count = 0;
            CountCommand.RaiseCanExecuteChanged();
        }

        private bool CountCommandCanExecute(object parameter = null)
        {
            return _count < _maxCount;
        }

        private void CountCommandExecute(object parameter)
        {
            _counting = !_counting;
            CountHandler(_counting);
        }

        private void CountHandler(bool flag)
        {
            var text = string.Empty;
            if (flag)
            {
                text = "开始计数";
                _countTimer.Start();
            }
            else
            {
                text = "停止计数";
                _countTimer.Stop();
            }
            PrintText(text);
        }

        private void PrintText(string text)
        {
            _displayer.AppendText(text + "\r\n");
        }
    }
}
