using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;

namespace iExt.WindowsBase.Tests.ViewModels.Media
{
    public class VisualTreeExtViewModel : BindableBase
    {
        public ICommand LoadedCommand { get; }

        public ICommand LoadedParentCommand { get; }

        public VisualTreeExtViewModel()
        {
            LoadedCommand = new DelegateCommand<DependencyObject>(LoadedCommandExecuteMethod);
            LoadedParentCommand = new DelegateCommand<DependencyObject>(LoadedParentCommandExecuteMethod);
        }

        private void LoadedParentCommandExecuteMethod(DependencyObject o)
        {
            o.ForeachVisualTree<DependencyObject>(p =>
            {
                Debug.WriteLine(p);
                return ForeachFlag.Normal;
            }, false);
        }

        private void LoadedCommandExecuteMethod(DependencyObject o)
        {
            o.ForeachVisualTree<DependencyObject>(p =>
            {
                Debug.WriteLine(p);
                return ForeachFlag.Normal;
            });
        }
    }
}

