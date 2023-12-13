using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace System.Windows.Markup
{
    /// <summary>
    /// 静态语言标记扩展
    /// </summary>
    public class LangExtension : MarkupExtension
    {
        private LangProvider _provider;

        /// <summary>
        /// 表示指向语言包文本的 <see cref="StaticExtension"/>
        /// </summary>
        public StaticExtension Key { get; set; }
        
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null != Key)
            {
                if (null == _provider)
                {
                    _provider = LangProvider.Register(Key.MemberType);
                }
                var binding = new Binding();
                binding.Source = _provider;
                binding.Path = new PropertyPath($"[{Key.Member}]");
                return binding.ProvideValue(serviceProvider);
            }

            return Key?.ProvideValue(serviceProvider);
        }
    }
}