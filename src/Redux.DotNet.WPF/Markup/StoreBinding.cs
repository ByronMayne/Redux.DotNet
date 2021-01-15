using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
        internal record Dependency(DependencyObject Object, DependencyProperty Property);


        private readonly string m_path;
        private Dependency m_dependency;
        private PropertyDescriptor m_propertyDescriptor;

        /// <summary>
        /// Gets or sets a value that indicates the direction of the data flow in the binding.
        /// </summary>
        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode { get; set; }

        /// <summary>
        /// Gets or sets a value that determines the timing of binding source updates.
        /// </summary>
        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        [DefaultValue(null)]
        public Type UpdateAction { get; set; }

        /// <summary>
        /// Gets the section this binding should use
        /// </summary>
        public string Section { get; set; }

        public StoreBinding(string path): base()
        {
            UpdateAction = null;
            m_path = path;
        }

        public StoreBinding(string path, Type actionType) : base()
        {
            UpdateAction = actionType;
            m_path = path;
        }

        private PropertyDescriptor GetPropertyDescriptor(Dependency dependency)
        {
            PropertyDescriptor propertyDescriptor = null;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(m_dependency.Object.GetType());

            for (int i = 0; i < properties.Count; i++)
            {
                if (propertyDescriptor == null && string.Equals(properties[i].Name, m_dependency.Property.Name, StringComparison.OrdinalIgnoreCase))
                {
                    propertyDescriptor = properties[i];
                    break;
                }
            }

            return propertyDescriptor;
        }



        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget valueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            if (valueTarget.TargetObject is FrameworkElement frameworkElement)
            {
                m_dependency = new Dependency((DependencyObject)valueTarget.TargetObject, (DependencyProperty)valueTarget.TargetProperty);


                IStore store = frameworkElement.GetStore();

                if(store is INotifyPropertyChanged notifyPropertyChanged)
                {
                    notifyPropertyChanged.PropertyChanged += OnStorePropertyChanged;
                }

                m_propertyDescriptor = GetPropertyDescriptor(m_dependency);


                m_propertyDescriptor.AddValueChanged(m_dependency.Object, new EventHandler(TargetPropertyChanged));


                Binding binding = new Binding($"{WPFConstants.STORE_STATE_NAME}.{m_path}")
                {
                    Source = store,
                    NotifyOnSourceUpdated = true,
                    UpdateSourceTrigger = UpdateSourceTrigger,
                    NotifyOnTargetUpdated = true,
                    Mode = Mode,
                };
                
                return binding.ProvideValue(serviceProvider);
            }

            return null;
        }

        private void OnStorePropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void TargetPropertyChanged(object sender, EventArgs e)
        {
            if (UpdateAction == null)
            {
                return;
            }

            // Raised whenever the store changes values
            ConstructorInfo constructor = UpdateAction.GetConstructors().First();

            object propertyValue = m_propertyDescriptor.GetValue(m_dependency.Object);

            ParameterInfo parameterInfo = constructor.GetParameters()[0];

            if (parameterInfo.ParameterType == typeof(int))
            {
                propertyValue = int.Parse((string)propertyValue);
            }

            IAction action = (IAction)constructor.Invoke(new object[] { propertyValue });
            ReduxDispatch.Dispatch(action);
        }
    }
}
