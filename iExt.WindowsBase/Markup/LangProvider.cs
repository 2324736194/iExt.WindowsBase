using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace System.Windows.Markup
{
    /// <summary>
    /// 语言包代理
    /// </summary>
    public class LangProvider : IRaisePropertyChanged
    {
        private static readonly Dictionary<Type, LangProvider> providers;
        private PropertyChangedEventHandler propertyChanged;
        private readonly ResourceManager manager;

        /// <summary>
        /// 设置时区
        /// </summary>
        public static CultureInfo Culture 
        { 
            get=>CultureInfo.CurrentCulture;
            set
            {
                CultureInfo.CurrentCulture = value;
                foreach (var provider in providers)
                {
                    // ReSharper disable once ExplicitCallerInfoArgument
                    provider.Value.RaisePropertyChanged(ComponentModelExt.NPN_This);
                }
            }
        }

        /// <summary>
        /// 获取语言包中对应名称的文本
        /// </summary>
        /// <param name="key"></param>
        public string this[string key] => manager.GetString(key, Culture);

        static LangProvider()
        {
            providers = new Dictionary<Type, LangProvider>();
        }

        private LangProvider(Type lang)
        {
            manager = GetManager(lang);
        }

        private ResourceManager GetManager(Type lang)
        {
            var property = lang.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public);
            if (null == property)   
            {
                throw new ArgumentOutOfRangeException(nameof(lang));
            }

            var propertyValue = property.GetValue(null);
            if (!(propertyValue is ResourceManager resourceManager))
            {
                throw new ArgumentOutOfRangeException(nameof(lang));
            }
            return resourceManager;
        }
        
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        void IRaisePropertyChanged.RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            propertyChanged?.Invoke(this, e);
        }

        public static LangProvider Register(Type lang)
        {
            if (!providers.TryGetValue(lang, out var provider))
            {
                provider = new LangProvider(lang);
                providers.Add(lang, provider);
            }

            return provider;
        }
    }
}