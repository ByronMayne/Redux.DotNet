using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ReduxSharp.Converters
{
    /// <summary>
    /// Allows for a string value to be serialized/deserizlied into a JToken instance of the string value.
    /// </summary>
    public class JTokenConverter : JsonConverter<JToken>
    {
        public override JToken ReadJson(JsonReader reader, Type objectType, JToken existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken token = JToken.ReadFrom(reader);

            if (token.Type == JTokenType.String)
            {
                return JToken.Parse(token.ToObject<string>());
            }

            return token;
        }

        public override void WriteJson(JsonWriter writer, JToken value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
