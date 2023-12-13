using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Tests;
using iExt.WindowsBase.Demo.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace iExt.WindowsBase.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //base.RegisterTypes(containerRegistry);
            var t = typeof(LangProvider);
            var events = t.GetEvents(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<TestsModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
