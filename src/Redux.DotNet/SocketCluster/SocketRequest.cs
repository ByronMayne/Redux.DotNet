using Newtonsoft.Json;
using WebSocket4Net;

namespace ReduxSharp.SocketCluster
{
    public class SocketRequest
    {
        [JsonProperty("event")]
        public string EventName { get; init; }

        [JsonProperty("cid")]
        public long CallId { get; }

        [JsonProperty("data")]
        public object Data { get; init; }

        public SocketRequest(long callId)
        {
            CallId = callId;
        }

        /// <summary>
        /// Sends this request to the given socket
        /// </summary>
        /// <param name="webSocket">The so</param>
        public void Send(WebSocket webSocket)
        {
            string json = JsonConvert.SerializeObject(this);
            webSocket.Send(json);
        }
    }
}
