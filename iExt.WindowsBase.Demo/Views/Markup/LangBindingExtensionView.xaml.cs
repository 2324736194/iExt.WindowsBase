using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace iExt.WindowsBase.Demo.Views.Markup
{
    /// <summary>
    /// LangBindingExtensionView.xaml 的交互逻辑
    /// </summary>
    public partial class LangBindingExtensionView : UserControl,IRaisePropertyChanged
    {
        public IReadOnlyCollection<string> LangKeys { get; }

        private string _langKey;
        public string LangKey
        {
            get => _langKey;
            set => this.SetValue(ref _langKey, value);
        }

        public LangBindingExtensionView()
        {
            InitializeComponent();
            LangKeys = GetLangKeys();
            DataContext = this;
        }

        private IReadOnlyList<string> GetLangKeys()
        {
            var properties = typeof(Demo.Langs.Legends).GetProperties(BindingFlags.Static | BindingFlags.Public);
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
