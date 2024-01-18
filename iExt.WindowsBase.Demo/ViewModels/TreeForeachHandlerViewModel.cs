using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace iExt.WindowsBase.Demo.ViewModels
{
    internal class TreeForeachHandlerViewModel: BindableBase
    {
        private TreeForeachCategory _foreachCategory;
        private TreeForeachMode _foreachMode;
        private string _foreachTarget;
        private TreeViewItem _item;
        private TextBox _tb;
        private ForeachFlag _flag;
        private int _maxIndex;
       
        public int MaxIndex
        {
            get => _maxIndex;
            set => SetProperty(ref _maxIndex, value);
        }
        
        public ForeachFlag Flag
        {
            get => _flag;
            set => SetProperty(ref _flag, value);
        }
    
        public string ForeachTarget
        {
            get => _foreachTarget;
            set => SetProperty(ref _foreachTarget, value);
        }
        
        public TreeForeachMode ForeachMode
        {
            get => _foreachMode;
            set => SetProperty(ref _foreachMode, value);
        }

        public TreeForeachCategory ForeachCategory
        {
            get => _foreachCategory;
            set => SetProperty(ref _foreachCategory, value);
        }
        
        public ICommand ForeachCommand { get; }

        public ICommand SelectedCommand { get; }
        
        public TreeForeachHandlerViewModel()
        {
            ForeachCommand = new DelegateCommand<TextBox>(ForeachCommandExecuteMethod);
            SelectedCommand = new DelegateCommand<RoutedPropertyChangedEventArgs<object>>(SelectedCommandExecuteMethod);
        }

        private void SelectedCommandExecuteMethod(RoutedPropertyChangedEventArgs<object> e)
        {
            switch (e.NewValue)
            {
                case TreeViewItem item:
                    _item = item;
                    ForeachTarget = item.Header.ToString();
                    break;
            }
        }
        
        private void ForeachCommandExecuteMethod(TextBox tb)
        {
            _tb = tb;
            _tb.Clear();
            var builder = new StringBuilder();
            switch (Flag)
            {
                case ForeachFlag.Break:
                    builder.AppendLine($"层级超过 {_maxIndex} 停止");
                    break;
                case ForeachFlag.Continue:
                    builder.AppendLine($"层级超过 {_maxIndex} 跳过");
                    break;
            }
            builder.AppendLine();
            _tb.AppendText(builder.ToString());
            var handler = new TreeForeachHandler();
            handler.ForeachCategory = ForeachCategory;
            handler.ForeachMode = ForeachMode;
            handler.FlagHandler =FlagHandler;
            handler.Handle(_item);
        }

        private ForeachFlag FlagHandler(int index, object element)
        {
            var indent = string.Empty;
            for (int i = 0; i < index; i++)
            {
                indent += "    ";
            }

            var builder = new StringBuilder();
            builder.AppendLine($"{indent}{index} >>> {element}");
            _tb.AppendText(builder.ToString());
            switch (Flag)
            {
                case ForeachFlag.Normal:
                    return Flag;
                case ForeachFlag.Break:
                case ForeachFlag.Continue:
                    if (index >= _maxIndex)
                    {
                        return Flag;
                    }

                    return ForeachFlag.Normal;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
