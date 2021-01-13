
namespace ReduxSharp.Redux.Actions
{
    internal class ExecuteAction : BaseAction, IAction
    {
        public const string ACTION_KEY = "devtools/executeAction";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;

        protected ExecuteAction() : base(EventType.Action)
        {
        }
    }
}
