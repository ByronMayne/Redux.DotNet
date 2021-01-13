namespace ReduxSharp
{
    public interface IAction
    {
        /// <summary>
        /// Gets a unique name for the action
        /// </summary>
        string Type { get; }
    }
}
