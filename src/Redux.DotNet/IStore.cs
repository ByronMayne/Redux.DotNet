using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace ReduxSharp
{
    public interface IStore
    {
        /// <summary>
        /// Returns the current state tree of your application. It is equal to the last value 
        /// returned by the store's reducer.
        /// </summary>
        object GetState();

        /// <summary>
        /// Dispatches an action. This is the only way to trigger a state change. The store's reducing 
        /// function will be called with the current getState() result and the given action synchronously. 
        /// Its return value will be considered the next state. It will be returned from getState() 
        /// from now on, and the change listeners will immediately be notified.
        /// </summary>
        /// <param name="action"></param>
        void Dispatch(IAction action);
    }


    public interface IStore<TState> : IStore
    {
        /// <summary>
        /// Returns the current state tree of your application. It is equal to the last value 
        /// returned by the store's reducer.
        /// </summary>
        new TState GetState();

        /// <summary>
        /// Adds a change listener. It will be called any time an action is dispatched, 
        /// and some part of the state tree may potentially have changed. You may then 
        /// call getState() to read the current state tree inside the callback.
        /// </summary>
        /// <param name="listener"></param>
        event Action<TState> StateChange;
    }
}
