using ReduxSharp;

namespace Redux.DotNet.DevTools
{
    internal class ToggleAction : DispatchAction, IAction
    {
        public const string ACTION_KEY = "monitor/toggle_action";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;
    }
}