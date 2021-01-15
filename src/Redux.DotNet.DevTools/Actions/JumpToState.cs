using ReduxSharp;

namespace Redux.DotNet.DevTools
{
    /// <summary>
    /// Event raised by the monitor when using the slider
    /// </summary>
    internal class JumpToState : DispatchAction, IAction
    {
        public const string ACTION_KEY = "monitor/jump_to_action";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;
    }
}