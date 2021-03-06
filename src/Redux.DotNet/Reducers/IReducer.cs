﻿using System.Threading.Tasks;

namespace ReduxSharp.Reducers
{
    public interface IAbstractReducer<TState> 
    { }


    public interface IAsyncReducer<TState> : IAbstractReducer<TState>
    {
        Task<TState> ReduceAsync(TState currentState, IAction action);
    }

    public interface IReducer<TState> : IAbstractReducer<TState>
    {
        TState Reduce(TState currentState, IAction action);
    }
}
