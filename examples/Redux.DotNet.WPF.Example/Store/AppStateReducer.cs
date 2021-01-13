using ReduxSharp.Reducers;
using ReduxSharp.Wpf.Store.Actions;

namespace ReduxSharp.Wpf.Store
{
    internal class AppStateReducer : IReducer<ApplicationState>
    {
        public ApplicationState Reduce(ApplicationState currentState, IAction action)
        {
            switch (action)
            {
                case DecrementCount decrementCount:
                    return currentState with { IntValue = currentState.IntValue - decrementCount.Amount };
                case IncrementCount incrementCount:
                    return currentState with { IntValue = currentState.IntValue + incrementCount.Amount };
            }

            return currentState;
        }
    }
}
