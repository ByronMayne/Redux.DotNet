using ReduxSharp;

namespace Redux.DotNet.Actions
{
    /// <summary>
    /// Action fired when Redux is being initialized
    /// </summary>
    public class InitializeAction : IAction
    {
        /// <inheritdoc cref="IAction"/>
        public string Type => "@@redux/INIT";
    }
}
