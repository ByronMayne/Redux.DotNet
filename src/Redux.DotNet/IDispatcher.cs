using System;

namespace ReduxSharp
{
    /// <summary>
    /// Defines a dispatcher that takes in an action and has the option 
    /// dothing nothing or completly replaces the action
    /// </summary>
    public interface IDispatcher<TState>
    {
        /// <summary>
        /// Invokes the dispatcher and returns back the same
        /// action or a new one.
        /// </summary>
        /// <param name="action">The action to process</param>
        /// <returns>The same action or the replaced one</returns>
        public TState Dispatch(IAction action, TState state);
    }
}
