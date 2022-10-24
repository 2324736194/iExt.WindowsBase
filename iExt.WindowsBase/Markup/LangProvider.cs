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
        private ResourceManager lang;
        private CultureInfo culture;
        private Assembly langAssembly;

        /// <summary>
        /// 语言包所在的程序集
        /// </summary>
        public Assembly LangAssembly
        {
            get => langAssembly;
            set
            {
                var changed = this.SetValue(ref langAssembly, value);
                if (changed)
                {
                    lang = GetLang(langAssembly);
                }
            }
        }

        /// <summary>
        /// 语言
        /// </summary>
        public CultureInfo Culture
        {
            get => culture;
            set
            {
                var changed = this.SetValue(ref culture, value);
                if (changed)
                {
                    var args = new PropertyChangedEventArgs(ComponentModelExt.NPN_This);
                    var raise = (IRaisePropertyChanged)this;
                    raise.RaisePropertyChanged(args);
                }
            }
        }

        /// <summary>
        /// 获取语言包中对应名称的文本
        /// </summary>
        /// <param name="key"></param>
        public string this[string key]
        {
            get
            {
                if (null == lang)
                {
                    return default;
                }

                var name = key.Replace('_', '.');
                var value = null == culture ? lang.GetString(name) : lang.GetString(name, culture);
                if (string.IsNullOrEmpty(value))
                {
                    value = name;
                }
                return value;
            }
        }

        private ResourceManager GetLang(Assembly assembly)
        {
            var names = assembly.GetManifestResourceNames();
            var lang = default(ResourceManager);
            foreach (var name in names)
            {
                if (!name.ToLower().EndsWith(".resources"))
                    continue;

                var resource = IO.Path.GetFileNameWithoutExtension(name);
                var resourceName = resource.Split('.').Last();
                if (resourceName.ToLower() == "lang")
                {
                    if (null == lang)
                    {
                        lang = new ResourceManager(resource, assembly);
                        continue;
                    }

                    throw new NotImplementedException("一个程序集中只允许添加一个语言包");
                }
            }

            return lang;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        void IRaisePropertyChanged.RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}