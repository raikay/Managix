namespace System.Text.Json.Serialization
{
    internal class TimeZoneInfoConverter : JsonConverter<TimeZoneInfo>
    {
        public override TimeZoneInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var name = reader.GetString();

            return TimeZoneInfo.FindSystemTimeZoneById(name);
        }

        public override void Write(Utf8JsonWriter writer, TimeZoneInfo value, JsonSerializerOptions options)
        {
            var text = value.Id;

            writer.WriteStringValue(text);
        }
    }
}
