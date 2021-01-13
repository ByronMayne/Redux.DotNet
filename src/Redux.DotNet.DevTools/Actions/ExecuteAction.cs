
using ReduxSharp;

namespace Redux.DotNet.DevTools
{
    internal class ExecuteAction : BaseAction, IAction
    {
        public const string ACTION_KEY = "monitor/executeAction";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;
    }
}
