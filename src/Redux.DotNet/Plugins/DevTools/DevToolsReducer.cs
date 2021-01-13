using ReduxSharp.Reducers;
using ReduxSharp.Redux;
using ReduxSharp.Redux.Actions;

namespace ReduxSharp.Plugins.DevTools
{
    public class DevToolsReducer<TState> : IReducer<TState>
    {
        private readonly ReduxConnection m_connection;

        public DevToolsReducer(ReduxConnection connection)
        {
            m_connection = connection;
        }

        public TState Reduce(TState currentState, IAction action)
        {
            switch (action)
            {
                case JumpToAction jumpToAction:
                    return jumpToAction.State.ToObject<TState>();
                case ToggleAction toggleAction:

                    break;

                case ExecuteAction execute:

                    break;
            }


            return currentState;
        }
    }
}
