namespace Managix.Common.Json
{
    public interface IJsonSerializer
    {
        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="value">The item.</param>
        /// <returns>Return the serialized object</returns>
        byte[] SerializeToUtf8Bytes<TValue>(TValue value);

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON string.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the value.</returns>
        string Serialize<TValue>(TValue value);

        /// <summary>
        /// Deserializes the specified bytes.
        /// </summary>
        /// <typeparam name="T">The type of the expected object.</typeparam>
        /// <param name="utf8Json">The serialized object.</param>
        /// <returns>
        /// The instance of the specified Item
        /// </returns>
        T Deserialize<T>(System.ReadOnlySpan<byte> utf8Json);

        /// <summary>
        /// Parses the text representing a single JSON value into an instance of the type specified by a generic type parameter.
        /// </summary>
        /// <typeparam name="T">The target type of the JSON value.</typeparam>
        /// <param name="json">The JSON text to parse.</param>
        /// <returns>A TValue representation of the JSON value.</returns>
        T Deserialize<T>(string json);

        /// <summary>
        /// Parses the text representing a single JSON value into an instance of a specified type.
        /// </summary>
        /// <param name="json">The JSON text to parse.</param>
        /// <param name="type">The type of the object to convert to and return.</param>
        /// <returns>A returnType representation of the JSON value.</returns>
        object Deserialize(string json, System.Type type);
    }
}
