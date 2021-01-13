namespace ReduxSharp
{
    public class ReduxDispatch
    {
        /// <summary>
        /// Gets the current store
        /// </summary>
        internal static IStore Store { get; set; }

        /// <summary>
        /// Dispatches the given action to Redux
        /// </summary>
        /// <param name="action"></param>
        public static void Dispatch(IAction action)
            => Store.Dispatch(action);
    }
}
