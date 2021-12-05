using System.Text.Json;

namespace Managix.Common.Json.SystemTextJson
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
        public byte[] SerializeToUtf8Bytes<TValue>(TValue value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, JapxSerializationOptions.Default);
        }

        public string Serialize<TValue>(TValue value)
        {
            if (value is string v)
            {
                return v;
            }
            return JsonSerializer.Serialize(value, JapxSerializationOptions.Default);
        }

        public T Deserialize<T>(System.ReadOnlySpan<byte> utf8Json)
        {
            return JsonSerializer.Deserialize<T>(utf8Json, JapxSerializationOptions.Default);
        }

        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(json, JapxSerializationOptions.Default);
        }

        public object Deserialize(string json, System.Type type)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return JsonSerializer.Deserialize(json, type, JapxSerializationOptions.Default);
        }
    }
}



