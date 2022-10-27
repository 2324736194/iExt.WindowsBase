using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iExt.WindowsBase.Tests.Views.Markup
{
    /// <summary>
    /// LangExtensionView.xaml 的交互逻辑
    /// </summary>
    public partial class LangExtensionView : UserControl
    {
        public LangExtensionView()
        {
            InitializeComponent();
        }

        private void LangChanged_OnClick(object sender, RoutedEventArgs e)
        {
            switch (LangProvider.Culture.Name)
            {
                case Langs.Chinese:
                    LangProvider.Culture = new CultureInfo(Langs.English);
                    break;
                case Langs.English:
                    LangProvider.Culture = new CultureInfo(Langs.Chinese);
                    break;
            }
        }
    }

    public static class Langs
    {
        public const string Chinese = "zh-CN";

        public const string English = "en";
    }   
}
