using StackExchange.Redis;

namespace Managix.Redis.Abstractions
{
    public partial interface IRedisDatabase
    {
        /// <summary>
        ///     Get the objects with the specified tag from Redis database
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="commandFlags">Behaviour markers associated with a given command</param>
        /// <typeparam name="T">The type of the class to add to Redis</typeparam>
        /// <returns>
        ///     Empty list if there are no results, otherwise the instance of T.
        ///     If a cache tag is not present on Redis the specified object into the returned IEnumerable will be null
        /// </returns>
        Task<IEnumerable<T>> GetByTagAsync<T>(string tag, CommandFlags commandFlags = CommandFlags.None);

        /// <summary>
        ///     Removes all specified keys by tag from Redis Database
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="flags">Behaviour markers associated with a given command</param>
        /// <returns>The number of items removed</returns>
        Task<long> RemoveByTagAsync(string tag, CommandFlags flags = CommandFlags.None);
    }
}
