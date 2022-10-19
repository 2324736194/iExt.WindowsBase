using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;

namespace iExt.WindowsBase.Tests.ViewModels
{
    public class LogicalTreeExtViewModel: BindableBase
    {
        public ICommand LoadedCommand { get; }

        public ICommand LoadedParentCommand { get; }

        public LogicalTreeExtViewModel()
        {
            LoadedCommand = new DelegateCommand<DependencyObject>(LoadedCommandExecuteMethod);
            LoadedParentCommand = new DelegateCommand<DependencyObject>(LoadedParentCommandExecuteMethod);
        }

        private void LoadedParentCommandExecuteMethod(DependencyObject o)
        {
            o.ForeachLogicalTree<DependencyObject>(p =>
            {
                Debug.WriteLine(p);
                return ForeachFlag.Normal;
            }, false);
        }

        private void LoadedCommandExecuteMethod(DependencyObject o)
        {
            o.ForeachLogicalTree<DependencyObject>(p =>
            {
                Debug.WriteLine(p);
                return ForeachFlag.Normal;
            });
        }
    }
}
