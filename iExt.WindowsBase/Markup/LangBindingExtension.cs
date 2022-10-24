using System.Linq;
using System.Windows.Data;

namespace System.Windows.Markup
{
    /// <summary>
    /// 动态语言包文本标记扩展 
    /// </summary>
    public class LangBindingExtension : LangMarkupExtension
    {
        private static readonly Type ownerType = typeof(LangBindingExtension);

        #region LangKey

        /// <summary>
        /// 获取或设置语言包中的资源关键字
        /// </summary>
        internal static readonly DependencyProperty LangKeyProperty =
            DependencyProperty.RegisterAttached("LangKey",
                typeof(string), ownerType,
                new PropertyMetadata(default(string), LangKeyPropertyChangedCallback));

        private static void LangKeyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var langMarkup = GetLangMarkup(d);
            langMarkup.SetLangBinding(d);
        }

        /// <summary>
        /// 设置 <see cref="LangKeyProperty"/> 的值
        /// </summary>
        internal static void SetLangKey(DependencyObject element, string value)
        {
            element.SetValue(LangKeyProperty, value);
        }

        /// <summary>
        /// 获取 <see cref="LangKeyProperty"/> 的值
        /// </summary>
        internal static string GetLangKey(DependencyObject element)
        {
            return (string)element.GetValue(LangKeyProperty);
        }

        #endregion

        #region TargetProperty

        /// <summary>
        /// 获取或设置语言绑定的目标属性
        /// </summary>
        internal static readonly DependencyProperty TargetPropertyProperty =
            DependencyProperty.RegisterAttached("TargetProperty",
                typeof(DependencyProperty), ownerType,
                new PropertyMetadata(default(DependencyProperty)));

        /// <summary>
        /// 设置 <see cref="TargetPropertyProperty"/> 的值
        /// </summary>
        internal static void SetTargetProperty(DependencyObject element, DependencyProperty value)
        {
            element.SetValue(TargetPropertyProperty, value);
        }

        /// <summary>
        /// 获取 <see cref="TargetPropertyProperty"/> 的值
        /// </summary>
        internal static DependencyProperty GetTargetProperty(DependencyObject element)
        {
            return (DependencyProperty)element.GetValue(TargetPropertyProperty);
        }

        #endregion

        #region LangMarkup

        /// <summary>
        /// 获取或设置当前元素的语言标记
        /// </summary>
        internal static readonly DependencyProperty LangMarkupProperty =
            DependencyProperty.RegisterAttached("LangMarkup",
                typeof(LangBindingExtension), ownerType,
                new PropertyMetadata(default(LangBindingExtension)));

        /// <summary>
        /// 设置 <see cref="LangMarkupProperty"/> 的值
        /// </summary>
        internal static void SetLangMarkup(DependencyObject element, LangBindingExtension value)
        {
            element.SetValue(LangMarkupProperty, value);
        }

        /// <summary>
        /// 获取 <see cref="LangMarkupProperty"/> 的值
        /// </summary>
        internal static LangBindingExtension GetLangMarkup(DependencyObject element)
        {
            return (LangBindingExtension)element.GetValue(LangMarkupProperty);
        }

        #endregion

        /// <summary>
        /// 此路径会与语言代理中的 Key 绑定
        /// </summary>
        public PropertyPath Key { get; set; }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null != Key &&
                serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provide)
            {
                switch (provide.TargetObject)
                {
                    case FrameworkElement element:
                        var events = WeakEventRelay.GetEvents<FrameworkElement>();
                        var eventName = nameof(FrameworkElement.DataContextChanged);
                        var e = events.Single(p => p.Name == eventName);
                        var relay = WeakEventRelay.Register(element, e, AddDataContextChanged);
                        relay.Add(new DependencyPropertyChangedEventHandler(OnDataContextChanged));
                        var targetProperty = provide.TargetProperty as DependencyProperty;
                        SetTargetProperty(element, targetProperty);
                        SetLangMarkup(element, this);
                        SetLangKeyBinding(element);
                        // ReSharper disable once AssignNullToNotNullAttribute
                        return BindingOperations.GetBinding(element, targetProperty);
                }
            }

            return base.ProvideValue(serviceProvider);
        }

        private void AddDataContextChanged(FrameworkElement element, WeakEventRelay relay)
        {
            element.DataContextChanged += (sender, args) =>
            {
                relay.Raise(sender, args);
            };
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetLangKeyBinding(element);
            }
        }

        private void SetLangKeyBinding(FrameworkElement element)
        {
            var binding = new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = Key,
                Source = element.DataContext
            };
            BindingOperations.SetBinding(element, LangKeyProperty, binding);
        }

        private void SetLangBinding(DependencyObject element)
        {
            var langKey = GetLangKey(element);
            var dependencyProperty = GetTargetProperty(element);
            var binding = CreateBinding(langKey);
            BindingOperations.SetBinding(element, dependencyProperty, binding);
        }
    }
}