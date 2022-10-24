namespace System.Windows.Markup
{
    /// <summary>
    /// 静态语言包文本标记扩展
    /// </summary>
    public class LangExtension : LangMarkupExtension
    {
        /// <summary>
        /// 语言包中的文本对应的关键字
        /// </summary>
        public StaticExtension Key { get; set; }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null != Key)
            {
                var binding = CreateBinding(Key.Member);
                return binding.ProvideValue(serviceProvider);
            }
            return base.ProvideValue(serviceProvider);
        }
    }
}