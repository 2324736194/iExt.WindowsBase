using System.Windows.Input;
using iExt.WindowsBase.Tests.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace iExt.WindowsBase.Tests.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private string title = "Prism Application";
        
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand LoadedCommand { get; }

        public MainWindowViewModel(IRegionManager manager)
        {
            regionManager = manager;
            LoadedCommand = new DelegateCommand(LoadedCommandExecuteMethod);
        }

        private void LoadedCommandExecuteMethod()
        {
            regionManager.RequestNavigate(MainWindow.ContentRegion, nameof(LogicalTreeExtView));
        }
    }
}
