using Redux.DotNet.Wpf;
using Redux.DotNet.Wpf.Store.Actions;
using ReduxSharp;
using ReduxSharp.Reducers;

namespace Redux.DotNet.WPF.Store
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
