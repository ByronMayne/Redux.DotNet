using ReduxSharp;

namespace Redux.DotNet.DevTools
{
    internal class JumpToAction : DispatchAction, IAction
    {
        public const string ACTION_KEY = "monitor/jump_to_action";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;
    }
}