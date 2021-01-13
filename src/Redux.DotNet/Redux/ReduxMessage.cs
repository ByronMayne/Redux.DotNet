using Newtonsoft.Json;
using System;


namespace ReduxSharp.Redux
{
    public class ReduxMessage
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

        public ReduxMessage()
        {
            Action = new ActionData();
        }
    }
}
