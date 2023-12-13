using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace iExt.WindowsBase.Demo.Views.Markup
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
