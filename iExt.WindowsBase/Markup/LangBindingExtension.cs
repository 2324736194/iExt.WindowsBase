using System.Events;
using System.Linq;
using System.Windows.Data;

namespace System.Windows.Markup
{
    /// <summary>
    /// 动态绑定语言包中的资源关键字
    /// </summary>
    public class LangBindingExtension : MarkupExtension
    {
        private static readonly Type _ownerType = typeof(LangBindingExtension);

        #region LangKey

        /// <summary>
        /// 获取或设置语言包中的资源关键字
        /// </summary>
        internal static readonly DependencyProperty _langKeyProperty =
            DependencyProperty.RegisterAttached("_langKey",
                typeof(string), _ownerType,
                new PropertyMetadata(default(string), LangKeyPropertyChangedCallback));

        private static void LangKeyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var langMarkup = Get_langMarkup(d);

            langMarkup.SetLangBinding(d);
        }

        /// <summary>
        /// 设置 <see cref="_langKeyProperty"/> 的值
        /// </summary>
        internal static void Set_langKey(DependencyObject element, string value)
        {
            element.SetValue(_langKeyProperty, value);
        }

        /// <summary>
        /// 获取 <see cref="_langKeyProperty"/> 的值
        /// </summary>
        internal static string Get_langKey(DependencyObject element)
        {
            return (string)element.GetValue(_langKeyProperty);
        }

        #endregion

        #region TargetProperty

        /// <summary>
        /// 获取或设置语言绑定的目标属性
        /// </summary>
        internal static readonly DependencyProperty _targetPropertyProperty =
            DependencyProperty.RegisterAttached("_targetProperty",
                typeof(DependencyProperty), _ownerType,
                new PropertyMetadata(default(DependencyProperty)));

        /// <summary>
        /// 设置 <see cref="_targetPropertyProperty"/> 的值
        /// </summary>
        internal static void Set_targetProperty(DependencyObject element, DependencyProperty value)
        {
            element.SetValue(_targetPropertyProperty, value);
        }

        /// <summary>
        /// 获取 <see cref="_targetPropertyProperty"/> 的值
        /// </summary>
        internal static DependencyProperty Get_targetProperty(DependencyObject element)
        {
            return (DependencyProperty)element.GetValue(_targetPropertyProperty);
        }

        #endregion

        #region LangMarkup

        /// <summary>
        /// 获取或设置当前元素的语言标记
        /// </summary>
        internal static readonly DependencyProperty _langMarkupProperty =
            DependencyProperty.RegisterAttached("_langMarkup",
                typeof(LangBindingExtension), _ownerType,
                new PropertyMetadata(default(LangBindingExtension)));

        /// <summary>
        /// 设置 <see cref="_langMarkupProperty"/> 的值
        /// </summary>
        internal static void Set_langMarkup(DependencyObject element, LangBindingExtension value)
        {
            element.SetValue(_langMarkupProperty, value);
        }

        /// <summary>
        /// 获取 <see cref="_langMarkupProperty"/> 的值
        /// </summary>
        internal static LangBindingExtension Get_langMarkup(DependencyObject element)
        {
            return (LangBindingExtension)element.GetValue(_langMarkupProperty);
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
                            var eventName = nameof(FrameworkElement.DataContextChanged);
                            var relay= element.RegisterWeakEvent(eventName);
                            relay.RegisterRaise(new DependencyPropertyChangedEventHandlerRegister());
                            relay.Add(new DependencyPropertyChangedEventHandler(OnDataContextChanged));
                            Set_targetProperty(element, targetProperty);
                            Set_langMarkup(element, this);
                            SetLangKeyBinding(element);
                            return BindingOperations.GetBinding(element, targetProperty);
                        }
                        break;
                }
            }

            return Designer.IsInDesignMode ? GetType().Name : default;
        }

        private void RaiseDataContextChanged(FrameworkElement owner, IWeakEventRelay relay)
        {
            owner.DataContextChanged += (sender, e) =>
            {
                relay.Raise(sender, e);
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
                Path = Path,
                Source = element.DataContext
            };
            BindingOperations.SetBinding(element, _langKeyProperty, binding);
        }

        private void SetLangBinding(DependencyObject element)
        {
            if (null == LangSource)
            {
                throw new ArgumentOutOfRangeException(nameof(LangSource));
            }
            var source = LangProvider.Register(LangSource);
            var langKey = Get_langKey(element);
            var dependencyProperty = Get_targetProperty(element);
            var binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath($"[{langKey}]");
            BindingOperations.SetBinding(element, dependencyProperty, binding);
        }
    }
}