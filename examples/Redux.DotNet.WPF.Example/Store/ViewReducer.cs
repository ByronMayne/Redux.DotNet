using ReduxSharp;
using ReduxSharp.Reducers;

namespace ProfileEditor
{
    internal class ViewReducer : IReducer<ViewState>
    {
        public ViewState Reduce(ViewState currentState, IAction action)
        {
            switch (action)
            {
                case ChangeViewSelectedTabAction changeView:
                    return currentState with { SelectedTab = changeView.Tab };

            }

            return currentState;
        }
    }
}
