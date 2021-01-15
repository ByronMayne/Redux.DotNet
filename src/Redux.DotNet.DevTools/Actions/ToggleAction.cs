using ReduxSharp;

namespace Redux.DotNet.DevTools
{

    /// <summary>
    /// Event raised by the monitor when using the inspector and clicking the `skip` button
    /// </summary>
    internal class ToggleAction : DispatchAction, IAction
    {
        public const string ACTION_KEY = "monitor/toggle_action";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;
    }
}