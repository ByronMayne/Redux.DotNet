using Newtonsoft.Json;
using System;

namespace Redux.DotNet.DevTools.SocketCluster
{
    internal class SocketMessage
    {
        public class ActionData
        {
            [JsonProperty("timestamp")]
            public DateTime TimeStamp { get; init; }

            [JsonProperty("type")]
            public string Type { get; init; }

            public ActionData()
            {
                Type = "update";
                TimeStamp = DateTime.Now;
            }
        }

        [JsonProperty("id")]
        public string SocketId { get; init; }

        [JsonProperty("type")]
        public string Type { get; init; }

        [JsonProperty("name")]
        public string ClientName { get; init; }

        [JsonProperty("instanceId")]
        public string InstanceId { get; init; }

        [JsonProperty("payload")]
        public object Payload { get; init; }

        [JsonProperty("action")]
        public ActionData Action { get; init; }

        public SocketMessage()
        {
            Action = new ActionData();
        }
    }
}
