using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace iExt.WindowsBase.Demo.ViewModels
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
