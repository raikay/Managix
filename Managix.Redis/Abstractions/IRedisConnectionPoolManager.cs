using StackExchange.Redis;

namespace Managix.Redis.Abstractions
{
    /// <summary>
    /// The service who handles the Redis connection pool.
    /// </summary>
    public interface IRedisConnectionPoolManager : IDisposable
    {
        ///// <summary>
        ///// Init the Redis connection.
        ///// </summary>
        //void Init();

        /// <summary>
        /// Get the Redis connection
        /// </summary>
        /// <returns>Returns an instance of<see cref="IConnectionMultiplexer"/>.</returns>
        IConnectionMultiplexer GetConnection();


        ///// <summary>
        /////     Gets the information about the connection pool
        ///// </summary>
        //ConnectionPoolInformation GetConnectionInformations();
    }
}
