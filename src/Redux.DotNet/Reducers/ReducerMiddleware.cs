using ReduxSharp.Middleware;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReduxSharp.Reducers
{
    /// <summary>
    /// This is the last peice of middlware that is invoked just
    /// before invokging the reducers 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    internal class ReducerMiddleware<TState> : IMiddleware
    {
        private IReadOnlyList<IReducer<TState>> m_reducers;

        public ReducerMiddleware(IEnumerable<IReducer<TState>> reducers)
            => m_reducers = new List<IReducer<TState>>(reducers);

        public async Task InvokeAsync(IActionContext actionContext)
        {
            ActionContext<TState> typedActionContext = (ActionContext<TState>)actionContext;
            TState state = typedActionContext.Store.GetState();
            IAction action = typedActionContext.Action;

            foreach (IAbstractReducer<TState> abstractReducer in m_reducers)
            {
                switch (abstractReducer)
                {
                    case IAsyncReducer<TState> asyncReducer:
                        state = await asyncReducer.ReduceAsync(state, action);
                        break;
                    case IReducer<TState> reducer:
                        state = reducer.Reduce(state, action);
                        break;
                }
            }

            typedActionContext.Result = state;
        }
    }
}
