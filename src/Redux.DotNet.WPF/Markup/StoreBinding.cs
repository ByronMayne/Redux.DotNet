using ReduxSharp.Reflection;
using ReduxSharp.WPF.Utility;
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
        private class Property
        {
            public PropertyDescriptor Descriptor { get; init; }
            public DependencyProperty DependencyProperty { get; init; }
            public DependencyObject DependencyObject { get; init; }
            public IStore Store { get; init; }

            /// <summary>
            /// Gets the property value
            /// </summary>
            /// <returns></returns>
            public object GetValue()
                => Descriptor.GetValue(DependencyObject);

            /// <summary>
            /// Adds a new event handler
            /// </summary>
            /// <param name="eventHandler"></param>
            public void AddEventHandler(EventHandler eventHandler)
                => Descriptor?.AddValueChanged(DependencyObject, eventHandler);
        }

        private bool m_ignoreNextValueChangeEvent;
        private readonly string m_path;
        private Property m_property;

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

        /// <summary>
        /// Gets or sets the action that will be preformed when this value
        /// updates. 
        /// </summary>
        [DefaultValue(null)]
        public Type Action { get; set; }

        /// <summary>
        /// Gets the section this binding should use
        /// </summary>
        public string Section { get; set; }

        public StoreBinding(string path) : base()
        {
            Action = null;
            m_path = path;
            m_ignoreNextValueChangeEvent = false;
        }

        /// <summary>
        /// Invoked when this instance is first created use it to setup the object
        /// </summary>
        /// <param name="serviceProvider">The provider for parameters</param>
        private void Initialize(IServiceProvider serviceProvider)
        {
            m_ignoreNextValueChangeEvent = true;

            IProvideValueTarget valueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            // DependencyObject: the instance that holds the value 
            FrameworkElement dependencyObject = (FrameworkElement)valueTarget.TargetObject;
            // DependencyProperty: The property or virtual property of the instance
            DependencyProperty dependencyProperty = (DependencyProperty)valueTarget.TargetProperty;
            // PropertyDescriptor: Like PropertyInfo however it can also point at 'virtual' properties 
            PropertyDescriptor propertyDescriptor = ComponentModelUtility.GetPropertyDescriptor(dependencyObject, dependencyProperty);
            // We we store the data for redux
            IStore store = dependencyObject.GetStore();

            if (store is INotifyPropertyChanged propertyChanged)
            {
                propertyChanged.PropertyChanged += OnStorePropertyChanged;
            }

            m_property = new Property()
            {
                DependencyObject = dependencyObject,
                DependencyProperty = dependencyProperty,
                Store = store,
                Descriptor = propertyDescriptor,
            };
        }



        /// <inheritdoc cref="MarkupExtension"/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (m_property == null)
            {
                Initialize(serviceProvider); ;
            }


            if (Action != null)
            {
                m_property.AddEventHandler(new EventHandler(OnPropertyChanged));
            }

            return GetStoreValue();
        }

        /// <summary>
        /// Raised whenever the store has a value that changes.
        /// </summary>
        /// <param name="sender">The store itself</param>
        /// <param name="e">The property path in the store that changed</param>
        private void OnStorePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == m_path)
            {
                object value = GetStoreValue();
                m_ignoreNextValueChangeEvent = true;
                m_property.DependencyObject.SetValue(m_property.DependencyProperty, value);
            }
        }


        /// <summary>
        /// Using the current store this fetches the value.
        /// </summary>
        private object GetStoreValue()
        {
            object state = m_property.Store.GetState();

            object value = ReflectionUtility.GetValueFromPath(state, m_path);

            return Convert.ChangeType(value, m_property.DependencyProperty.PropertyType);
        }

        private void OnPropertyChanged(object sender, EventArgs e)
        {
            if(m_ignoreNextValueChangeEvent)
            {
                m_ignoreNextValueChangeEvent = false;
                return;
            }

            // Raised whenever the store changes values
            ConstructorInfo constructor = Action.GetConstructors().First();

            object propertyValue = m_property.GetValue();

            ParameterInfo parameterInfo = constructor.GetParameters()[0];

            propertyValue = Convert.ChangeType(propertyValue, parameterInfo.ParameterType);

            IAction action = (IAction)constructor.Invoke(new object[] { propertyValue });
            ReduxDispatch.Dispatch(action);
        }
    }
}
