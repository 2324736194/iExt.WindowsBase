using System.Windows.Data;

namespace System.Windows.Markup
{
    /// <summary>
    /// 语言包标记扩展，主要用于应用程序全球化
    /// </summary>
    public abstract class LangMarkupExtension : MarkupExtension
    {
        /// <summary>
        /// 默认语言代理
        /// </summary>
        public static LangProvider DefaultProvider { get; set; }

        /// <summary>
        /// 指定的语言代理，此属性的优先级大于 <see cref="DefaultProvider"/>
        /// </summary>
        public LangProvider Provider { get; set; }

        static LangMarkupExtension()
        {
            DefaultProvider =   new LangProvider()
            {
                LangAssembly = Application.Current.GetType().Assembly
            };
        }

        /// <inheritdoc />
        protected LangMarkupExtension()
        {
            Provider = DefaultProvider;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // 如果，当前对象在 VS 中的设计器中使用时，默认显示类名。
            if (Designer.IsInDesignMode)
            {
                return GetType().Name;
            }

            return this;
        }

        /// <summary>
        /// 绑定语言代理
        /// </summary>
        /// <param name="key">语言包中的资源 Key</param>
        /// <returns></returns>
        protected Binding CreateBinding(string key)
        {
            var binding = new Binding();
            binding.Source = Provider;
            binding.Path = new PropertyPath($"[{key}]");
            return binding;
        }
    }
}