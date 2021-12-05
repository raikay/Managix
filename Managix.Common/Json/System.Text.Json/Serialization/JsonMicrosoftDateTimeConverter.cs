namespace System.Text.Json.Serialization
{
    public class JsonMicrosoftDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly DateTime s_Epoch = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0), DateTimeKind.Utc);

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return s_Epoch.AddMilliseconds(long.Parse(reader.GetString().Replace("/Date(", string.Empty).Replace("+0800)/", string.Empty))).AddHours(8);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var unixTime = Convert.ToInt64((value.ToUniversalTime() - s_Epoch).TotalMilliseconds);
            var formatted = FormattableString.Invariant($"/Date({unixTime}+0800)/");
            writer.WriteStringValue(formatted);
        }
    }
}
