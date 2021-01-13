namespace ReduxSharp.Redux.Actions
{
    internal class JumpToAction : DispatchAction, IAction
    {
        public const string ACTION_KEY = "devtools/jump_to_action";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;

        protected JumpToAction() : base(ActionTypes.JumpToAction)
        {
        }
    }
}