using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReduxSharp;
using ReduxSharp.Converters;

namespace Redux.DotNet.DevTools
{
    internal class DispatchAction : BaseAction, IAction
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

        /// <inheritdoc cref="IAction"/>
        public string Type => "monitor/dispatchAction";
    }
}
