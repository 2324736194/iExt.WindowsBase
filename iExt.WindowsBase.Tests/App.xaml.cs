using iExt.WindowsBase.Tests.Views;
using Prism.Ioc;
using System.Windows;
using iExt.WindowsBase.Tests.ViewModels;

namespace iExt.WindowsBase.Tests
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Register(containerRegistry,this);
        }

        void Register(IContainerRegistry containerRegistry, Application application)
        {
            var applicationType = application.GetType();
            var applicationNamespace = applicationType.Namespace;
            var viewsNamespace = $"{applicationNamespace}.Views";
            var types = applicationType.Assembly.GetTypes();
            foreach (var type in types)
            {
                // ReSharper disable once PossibleNullReferenceException
                if (!type.Namespace.StartsWith(viewsNamespace))
                    continue;
                if (type.Name.EndsWith("Dialog") || type.Name.EndsWith("View"))
                {
                    containerRegistry.RegisterForNavigation(type, type.Name);
                }
            }
        }
    }

   
}
