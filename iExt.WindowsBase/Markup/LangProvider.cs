using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Events;
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
        private static readonly Dictionary<Type, LangProvider> _providers;
        private readonly IWeakEventRelay _relayPropertyChanged;
        private readonly ResourceManager _manager;

        /// <summary>
        /// 设置时区
        /// </summary>
        public static CultureInfo Culture 
        { 
            get=>CultureInfo.CurrentCulture;
            set
            {
                CultureInfo.CurrentCulture = value;
                foreach (var provider in _providers)
                {
                    // ReSharper disable once ExplicitCallerInfoArgument
                    provider.Value.RaisePropertyChanged(ObjectModelExt.NPN_This);
                }
            }
        }

        /// <summary>
        /// 获取语言包中对应名称的文本
        /// </summary>
        /// <param name="key"></param>
        public string this[string key] => _manager.GetString(key, Culture);

        static LangProvider()
        {
            _providers = new Dictionary<Type, LangProvider>();
        }

        private LangProvider(Type lang)
        {
            _manager = GetManager(lang);
            _relayPropertyChanged = this.GetRelayPropertyChanged();
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
            add => _relayPropertyChanged.Add(value);
            remove => _relayPropertyChanged.Remove(value);
        }

        void IRaisePropertyChanged.RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            _relayPropertyChanged.Raise(this, e);
        }

        /// <summary>
        /// 注册语言代理
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static LangProvider Register(Type lang)
        {
            if (!_providers.TryGetValue(lang, out var provider))
            {
                provider = new LangProvider(lang);
                _providers.Add(lang, provider);
            }

            return provider;
        }
    }
}