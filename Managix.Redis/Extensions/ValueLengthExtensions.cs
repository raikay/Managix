using Managix.Common.Json;

namespace Managix.Redis.Extensions
{
    internal static class ValueLengthExtensions
    {
        public static IEnumerable<KeyValuePair<string, byte[]>> OfValueInListSize<T>(this IEnumerable<Tuple<string, T>> items, IJsonSerializer serializer, uint maxValueLength)
        {
            using var iterator = items.GetEnumerator();

            while (iterator.MoveNext())
            {
                yield return new KeyValuePair<string, byte[]>(
                    iterator.Current.Item1,
                    iterator.Current.Item2.SerializeItem(serializer).CheckLength(maxValueLength, iterator.Current.Item1));
            }
        }

        public static byte[] OfValueSize<T>(this T value, IJsonSerializer serializer, uint maxValueLength, string key) => serializer.SerializeToUtf8Bytes(value).CheckLength(maxValueLength, key);

        private static byte[] SerializeItem<T>(this T item, IJsonSerializer serializer) => serializer.SerializeToUtf8Bytes(item);

        private static byte[] CheckLength(this byte[] byteArray, uint maxValueLength, string paramName)
        {
            if (maxValueLength > default(uint) && byteArray.Length > maxValueLength)
                throw new ArgumentException("value cannot be longer than the MaxValueLength", paramName);

            return byteArray;
        }
    }
}
