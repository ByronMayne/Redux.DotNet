using ReduxSharp;
using System.ComponentModel;

namespace ReduxSharp.WPF
{
    public class WPFStore<T> : Store<T>, INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        public WPFStore(T initialState, ActionDispatchDelegate dispatch) : base(initialState, dispatch)
        {
        }

        protected override void AfterDispatch()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(WPFConstants.STORE_STATE_NAME));
        }
    }
}
