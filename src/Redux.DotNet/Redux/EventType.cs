using Newtonsoft.Json;

namespace ReduxSharp.Redux
{
    public enum EventType
    {
        /// <summary>
        /// A monitor action dispatched on Redux DevTools monitor
        /// </summary>
        [JsonProperty("DISPATCH")]
        Dispatch,

        /// <summary>
        /// The user requested to dispatch an action remotely
        /// </summary>
        [JsonProperty("ACTION")]
        Action,

        /// <summary>
        /// A monitor was opened. You could handle this event in order not to do extra tasks when the app is not monitored.
        /// </summary>
        [JsonProperty("START")]
        Start,

        /// <summary>
        /// A monitor was closed. You can take this as no need to send data to the monitor. 
        /// I there are several monitors and one was closed, all others will send START 
        /// event to acknowledge that we still have to send data.
        /// </summary>
        [JsonProperty("STOP")]
        Stop
    }
}
