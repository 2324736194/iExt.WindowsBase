using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iExt.WindowsBase.Tests.Views.Markup
{
    /// <summary>
    /// LangBindingExtensionView.xaml 的交互逻辑
    /// </summary>
    public partial class LangBindingExtensionView : UserControl,IRaisePropertyChanged
    {
        public IReadOnlyCollection<string> LangKeys { get; }

        private string langKey;
        public string LangKey
        {
            get => langKey;
            set => this.SetValue(ref langKey, value);
        }

        public LangBindingExtensionView()
        {
            InitializeComponent();
            LangKeys = GetLangKeys();
            DataContext = this;
        }

        private IReadOnlyList<string> GetLangKeys()
        {
            var properties = typeof(Tests.Langs.Legends).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var propertyType = typeof(string);
            return properties
                .Where(p => p.PropertyType == propertyType)
                .Select(p=>p.Name)
                .ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
