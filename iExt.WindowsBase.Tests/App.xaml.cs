using System.Globalization;
using iExt.WindowsBase.Tests.Views;
using Prism.Ioc;
using System.Windows;
using iExt.Windows.Tests.Basics;
using iExt.WindowsBase.Tests.Langs;
using Prism.Modularity;
using System.Windows.Markup;

namespace iExt.WindowsBase.Tests
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(this);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<BasicsModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
