using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReduxSharp.Json
{
    public static class JsonUtility
    {
        /// <summary>
        /// Converts a json element into a given class type
        /// </summary>
        /// <typeparam name="T">Type to to convert too</typeparam>
        /// <param name="element">The element we are converting</param>
        /// <param name="options">The options to conver too</param>
        /// <returns>The result</returns>
        public static T To<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            ArrayBufferWriter<byte> bufferWriter = new ArrayBufferWriter<byte>();
            using (Utf8JsonWriter writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }
            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        public static JsonElement ToJsonElement<T>(this T instance, JsonSerializerOptions options = null)
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes<T>(instance, options);
            return JsonDocument.Parse(bytes).RootElement;
        }
    }
}
