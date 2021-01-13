using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReduxSharp.Converters;

namespace ReduxSharp.Redux.Actions
{
    internal class DispatchAction : BaseAction
    {
        /// <summary>
        /// Gets the state of the event
        /// </summary>
        [JsonProperty("state")]
        [JsonConverter(typeof(JTokenConverter))]
        public JToken State { get; private set; }

        /// <summary>
        /// Gets the instance Id of the event
        /// </summary>
        [JsonProperty("instanceId")]
        public string InstanceId { get; private set; }

        /// <summary>
        /// Gets the type of dispatch event that this is
        /// </summary>
        public ActionTypes DispatchType { get; }

        protected DispatchAction(ActionTypes dispatchType) : base(EventType.Dispatch)
        {
            DispatchType = dispatchType;
        }
    }
}
