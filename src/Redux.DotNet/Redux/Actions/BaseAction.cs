namespace ReduxSharp.Redux.Actions
{
    internal class BaseAction
    {
        /// <summary>
        /// Gets the type of event this action belongs too
        /// </summary>
        public EventType EventType { get; }

        protected BaseAction(EventType eventType)
        {
            EventType = eventType;
        }
    }
}