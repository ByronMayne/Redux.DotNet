using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace ReduxSharp.WPF.Markup
{
    public class ActionBinding : MarkupExtension
    {
        private readonly Type m_actionType;
        private readonly string m_accessor;
        private object m_target;

        public ActionBinding(Type actionType, string accessor) 
        {
            m_actionType = actionType;
            m_accessor = accessor;
        }

        


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            EventInfo eventInfo = target.TargetProperty as EventInfo;
            if (eventInfo == null)
                throw new InvalidOperationException("The target property must be an event");
            m_target = target.TargetObject;


            TextChangedEventHandler eventHandler = OnInvoked;

            return eventHandler;
        }

        private void OnInvoked(object target, TextChangedEventArgs textChanged)
        {
            ConstructorInfo constructor = m_actionType.GetConstructor(new[] { typeof(int) });
            TextBox textBox = (TextBox)target;
            IAction instance = (IAction)constructor.Invoke(new object[] { int.Parse(textBox.Text) });
            ReduxDispatch.Dispatch(instance);
        }


        //static object GetHandler(object instance, EventInfo eventInfo)
        //{
        //    MethodInfo invokeTarget = typeof(ActionBinding).GetMethod(nameof(OnInvoked), BindingFlags.Instance | BindingFlags.NonPublic);

        //    DynamicMethod handler = new DynamicMethod($"___{eventInfo.Name}__", typeof(void), GetParameterTypes(eventInfo));
        //    ILGenerator il = handler.GetILGenerator();
        //    il.Emit(OpCodes.Nop);
        //    il.Emit(OpCodes.Ldarg_0);
        //    il.Emit(OpCodes.Call, invokeTarget);
        //    il.Emit(OpCodes.Nop);
        //    il.Emit(OpCodes.Ret);
           
        //    return handler.CreateDelegate(eventInfo.EventHandlerType);

        //}

        //static Type[] GetParameterTypes(EventInfo eventInfo)
        //{
        //    var invokeMethod = eventInfo.EventHandlerType.GetMethod("Invoke");
        //    return invokeMethod.GetParameters().Select(p => p.ParameterType).ToArray();
        //}

        //static object GetDataContext(object target)
        //{
        //    var depObj = target as DependencyObject;
        //    if (depObj == null)
        //        return null;

        //    return depObj.GetValue(FrameworkElement.DataContextProperty)
        //        ?? depObj.GetValue(FrameworkContentElement.DataContextProperty);
        //}
    }
}
