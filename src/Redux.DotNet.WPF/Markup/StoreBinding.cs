using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ReduxSharp.WPF.Markup
{
    /// <summary>
    /// Used to bind controls directly to the most recent store provider
    /// </summary>
    public class StoreBinding : MarkupExtension
    {
        private readonly string m_path;

        public StoreBinding(string path) : base()
        {
            m_path = path;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget valueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            if (valueTarget.TargetObject is FrameworkElement frameworkElement)
            {
                IStore store = frameworkElement.GetStore();

                Binding binding = new Binding($"{WPFConstants.STORE_STATE_NAME}.{m_path}")
                {
                    Source = store,
                };

                return binding.ProvideValue(serviceProvider);
            }

            return null;
        }
    }
}
