using Redux.DotNet;
using ReduxSharp.Reflection;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Threading;

namespace ReduxSharp.WPF
{
    public class WPFStore<T> : Store<T>, INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Dispatcher m_dispatcher;

        public WPFStore(T initialState, ActionDispatchDelegate dispatch) : base(initialState, dispatch)
        {
            m_dispatcher = Dispatcher.CurrentDispatcher;
        }

        /// <inheritdoc cref="Store{TState}"/>
        protected override void UpdateStore(T newState)
        {
            T beforeUpdateState = State;

            base.UpdateStore(newState);

            if (!ReferenceEquals(beforeUpdateState, State))
            {
                m_dispatcher.Invoke(() =>
                {
                    foreach (string difference in ReflectionUtility.GetDifferentPropertyNames<T>(beforeUpdateState, State))
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(difference));
                    }
                });
            }
        }
    }
}
