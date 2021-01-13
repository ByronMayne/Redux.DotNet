namespace ReduxSharp.Redux.Actions
{
    internal class ToggleAction : DispatchAction, IAction
    {
        public const string ACTION_KEY = "devtools/toggle_action";

        /// <inheritdoc cref="IAction"/>
        public string Type => ACTION_KEY;


        protected ToggleAction() : base(ActionTypes.JumpToAction)
        {
        }
    }
}