using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Redux.DotNet.DevTools.SocketCluster
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum ReportType
    {
        [EnumMember(Value ="STATE")]
        State,
        [EnumMember(Value ="ACTION")]
        Action,
        [EnumMember(Value = "STATES")]
        States,
        [EnumMember(Value = "ACTIONS")]
        Actions
    }
}
