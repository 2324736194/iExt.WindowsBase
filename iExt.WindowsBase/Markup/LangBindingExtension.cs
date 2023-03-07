using System.Linq;
using System.Windows.Data;

namespace System.Windows.Markup
{
    /// <summary>
    /// 动态绑定语言包中的资源关键字
    /// </summary>
    public class LangBindingExtension : MarkupExtension
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
        /// 语言包
        /// </summary>
        public Type LangSource { get; set; }

        /// <summary>
        /// 资源关键字的属性路径，数据绑定参数如下：
        /// <para>- 扩展标记所在元素的数据上下文 </para>
        /// <para>- 仅支持 <see cref="BindingMode.OneWay"/></para>
        /// </summary>
        public PropertyPath Path { get; set; }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(IProvideValueTarget));
            if (service is IProvideValueTarget provide)
            {
                switch (provide.TargetObject)
                {
                    case FrameworkElement element:
                        if (provide.TargetProperty is DependencyProperty targetProperty)
                        {
                            var events = WeakEventRelay.GetEvents<FrameworkElement>();
                            var e = events.Single(p => p.Name == nameof(FrameworkElement.DataContextChanged));
                            var eHandler = new DependencyPropertyChangedEventHandler(OnDataContextChanged);
                            var relay= element.RegisterWeakEvent(e, RegisterDataContextChanged);
                            relay.Add(eHandler);
                            SetTargetProperty(element, targetProperty);
                            SetLangMarkup(element, this);
                            SetLangKeyBinding(element);
                            return BindingOperations.GetBinding(element, targetProperty);
                        }
                        break;
                }
            }

            return Designer.IsInDesignMode ? GetType().Name : default;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetLangKeyBinding(element);
            }
        }

        private void RegisterDataContextChanged(FrameworkElement owner, WeakEventRelay relay)
        {
            owner.DataContextChanged += (sender, e) =>
            {
                relay.Raise(sender, e);
            };
        }

        private void SetLangKeyBinding(FrameworkElement element)
        {
            var binding = new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = Path,
                Source = element.DataContext
            };
            BindingOperations.SetBinding(element, LangKeyProperty, binding);
        }

        private void SetLangBinding(DependencyObject element)
        {
            if (null == LangSource)
            {
                throw new ArgumentOutOfRangeException(nameof(LangSource));
            }
            var source = LangProvider.Register(LangSource);
            var langKey = GetLangKey(element);
            var dependencyProperty = GetTargetProperty(element);
            var binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath($"[{langKey}]");
            BindingOperations.SetBinding(element, dependencyProperty, binding);
        }
    }
}