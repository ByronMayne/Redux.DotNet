
using Redux.DotNet.Actions;
using ReduxSharp;
using System;

namespace Redux.DotNet
{
    public class Store<TState> : IStore<TState>
    {
        private Action<TState> m_stateChanged;
        private readonly ActionDispatchDelegate m_asyncDispatch;

        /// <summary>
        /// Gets the current state object in the store 
        /// </summary>
        public TState State { get; private set; }

        /// <summary>
        /// Raised whenever the state changes 
        /// </summary>
        public event Action<TState> StateChange
        {
            add => m_stateChanged += value;
            remove => m_stateChanged -= value;
        }

        /// <summary>
        /// Creates a new instance of a store
        /// </summary>
        /// <param name="initialState">The initial value of the state</param>
        /// <param name="reducers">The reducers we should use</param>
        /// <param name="middlewares">The middleware involved</param>
        public Store(TState initialState, ActionDispatchDelegate dispatch)
        {
            State = initialState;
            m_asyncDispatch = dispatch;
            ReduxDispatch.Store = this;

            Dispatch(new InitializeAction());
        }

        public TState GetState()
            => State;

        public void Dispatch(IAction action)
        {
            ActionContext<TState> context = new ActionContext<TState>
            {
                Action = action,
                Store = this,
                Result = GetState(),
            };

            Action<TState> listeners = m_stateChanged;

            Dispatch(context);

            UpdateStore(context.Result);

            listeners?.Invoke(State);
        }

        /// <summary>
        /// Invoked when going to dispatch an action.
        /// </summary>
        /// <param name="context">The action context that is going to be invoked</param>
        protected virtual void Dispatch(ActionContext<TState> context)
            => m_asyncDispatch(context).Wait();

        /// <summary>
        /// Invoked when the store should have it's value changed.
        /// </summary>
        protected virtual void UpdateStore(TState newState)
            => State = newState;

        object IStore.GetState()
            => GetState();
    }
}
