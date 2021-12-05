using Managix.Common.Json;
using Managix.Redis.Configuration;
using Managix.Redis.Implementations;
using Microsoft.Extensions.Logging;

namespace Managix.Redis.Abstractions
{
    /// <inheritdoc/>
    public class RedisClient : IRedisClient
    {
        private readonly IRedisConnectionPoolManager connectionPoolManager;
        private readonly RedisOptions redisConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisClient"/> class.
        /// </summary>
        /// <param name="connectionPoolManager">An instance of the <see cref="IRedisConnectionPoolManager" />.</param>
        /// <param name="serializer">An instance of the <see cref="IJsonSerializer" />.</param>
        /// <param name="redisConfiguration">An instance of the <see cref="RedisOptions" />.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public RedisClient(
            IRedisConnectionPoolManager connectionPoolManager,
            IJsonSerializer serializer,
            RedisOptions redisConfiguration,
            ILoggerFactory loggerFactory)
        {
            this.connectionPoolManager = connectionPoolManager;
            Serializer = serializer;
            this.redisConfiguration = redisConfiguration;
            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        public IRedisDatabase Db0 => GetDb(0);

        /// <inheritdoc/>
        public IRedisDatabase Db1 => GetDb(1);

        /// <inheritdoc/>
        public IRedisDatabase Db2 => GetDb(2);

        /// <inheritdoc/>
        public IRedisDatabase Db3 => GetDb(3);

        /// <inheritdoc/>
        public IRedisDatabase Db4 => GetDb(4);

        /// <inheritdoc/>
        public IRedisDatabase Db5 => GetDb(5);

        /// <inheritdoc/>
        public IRedisDatabase Db6 => GetDb(6);

        /// <inheritdoc/>
        public IRedisDatabase Db7 => GetDb(7);

        /// <inheritdoc/>
        public IRedisDatabase Db8 => GetDb(8);

        /// <inheritdoc/>
        public IRedisDatabase Db9 => GetDb(9);

        /// <inheritdoc/>
        public IRedisDatabase Db10 => GetDb(10);

        /// <inheritdoc/>
        public IRedisDatabase Db11 => GetDb(11);

        /// <inheritdoc/>
        public IRedisDatabase Db12 => GetDb(12);

        /// <inheritdoc/>
        public IRedisDatabase Db13 => GetDb(13);

        /// <inheritdoc/>
        public IRedisDatabase Db14 => GetDb(14);

        /// <inheritdoc/>
        public IRedisDatabase Db15 => GetDb(15);

        /// <inheritdoc/>
        public IRedisDatabase Db16 => GetDb(16);

        /// <inheritdoc/>
        public IJsonSerializer Serializer { get; }

        /// <inheritdoc/>
        public IRedisDatabase GetDb(int dbNumber, string keyPrefix = null)
        {
            if (string.IsNullOrEmpty(keyPrefix))
            {
                keyPrefix = redisConfiguration.KeyPrefix;
            }
            if (!string.IsNullOrEmpty(keyPrefix) && !keyPrefix.EndsWith(":"))
            {
                keyPrefix += ":";
            }
            if (keyPrefix == "~")
            {
                keyPrefix = null;
            }
            return new RedisDatabase(
                connectionPoolManager,
                Serializer,
                redisConfiguration.ServerEnumerationStrategy,
                dbNumber,
                redisConfiguration.MaxValueLength,
                keyPrefix,
                _loggerFactory);
        }

        /// <inheritdoc/>
        public IRedisDatabase GetDbFromConfiguration()
        {
            return GetDb(redisConfiguration.Database, redisConfiguration.KeyPrefix);
        }
    }
}
