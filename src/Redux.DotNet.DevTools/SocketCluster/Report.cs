using Newtonsoft.Json;
using System;

namespace Redux.DotNet.DevTools.SocketCluster
{

    internal class Report
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

        /// <summary>
        /// Briefly what happened
        /// </summary>
        [JsonProperty("title")]
        public string Tile { get; init; }

        /// <summary>
        /// Details supplied by the user
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; init; }

        [JsonProperty("id")]
        public string SocketId { get; init; }

        [JsonProperty("type")]
        public ReportType Type { get; init; }

        [JsonProperty("name")]
        public string ClientName { get; init; }

        [JsonProperty("instanceId")]
        public string InstanceId { get; init; }

        /// <summary>
        /// Serialized actions or the state or both, which should be loaded the application to reproduce the exact behavior
        /// </summary>
        [JsonProperty("payload")]
        public object Payload { get; init; }

        /// <summary>
        /// The last dispatched action before the report was sent
        /// </summary>
        [JsonProperty("action")]
        public ActionData Action { get; init; }

        public Report()
        {
            Action = new ActionData();
        }
    }
}
