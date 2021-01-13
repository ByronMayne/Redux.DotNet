using ReduxSharp;
using ReduxSharp.Reducers;

namespace Redux.DotNet.DevTools
{
    internal class Reducer<TState> : IReducer<TState>
    {
        private readonly ReduxConnection m_connection;

        public Reducer(ReduxConnection connection)
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
