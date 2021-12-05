
using Managix.Redis.Abstractions;
using StackExchange.Redis;

namespace Managix.Redis.Implementations
{
    public partial class RedisDatabase : IRedisDatabase
    {
        /// <inheritdoc/>
        public Task<long> PublishAsync<T>(RedisChannel channel, T message, CommandFlags flags = CommandFlags.None)
        {
            var sub = _connectionPoolManager.GetConnection().GetSubscriber();
            return sub.PublishAsync(channel, Serializer.Serialize(message), flags);
        }

        /// <inheritdoc/>
        public Task SubscribeAsync<T>(RedisChannel channel, Func<T, Task> handler, CommandFlags flags = CommandFlags.None)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var sub = _connectionPoolManager.GetConnection().GetSubscriber();

            return sub.SubscribeAsync(channel, async (redisChannel, value) => await handler(Serializer.Deserialize<T>(value)).ConfigureAwait(false), flags);
        }

        /// <inheritdoc/>
        public Task UnsubscribeAsync<T>(RedisChannel channel, Func<T, Task> handler, CommandFlags flags = CommandFlags.None)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var sub = _connectionPoolManager.GetConnection().GetSubscriber();
            return sub.UnsubscribeAsync(channel, (redisChannel, value) => handler(Serializer.Deserialize<T>(value)), flags);
        }

        /// <inheritdoc/>
        public Task UnsubscribeAllAsync(CommandFlags flags = CommandFlags.None)
        {
            var sub = _connectionPoolManager.GetConnection().GetSubscriber();
            return sub.UnsubscribeAllAsync(flags);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateExpiryAsync(string key, DateTimeOffset expiresAt, CommandFlags flags = CommandFlags.None)
        {
            if (await Database.KeyExistsAsync(key).ConfigureAwait(false))
                return await Database.KeyExpireAsync(key, expiresAt.UtcDateTime.Subtract(DateTime.UtcNow), flags).ConfigureAwait(false);

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateExpiryAsync(string key, TimeSpan expiresIn, CommandFlags flags = CommandFlags.None)
        {
            if (await Database.KeyExistsAsync(key).ConfigureAwait(false))
                return await Database.KeyExpireAsync(key, expiresIn, flags).ConfigureAwait(false);

            return false;
        }

        /// <inheritdoc/>
        public async Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, DateTimeOffset expiresAt, CommandFlags flags = CommandFlags.None)
        {
            var tasks = new Task<bool>[keys.Length];

            for (var i = 0; i < keys.Length; i++)
                tasks[i] = UpdateExpiryAsync(keys[i], expiresAt.UtcDateTime, flags);

            await Task.WhenAll(tasks).ConfigureAwait(false);

            var results = new Dictionary<string, bool>(keys.Length, StringComparer.Ordinal);

            for (var i = 0; i < keys.Length; i++)
                results.Add(keys[i], tasks[i].Result);

            return results;
        }

        /// <inheritdoc/>
        public async Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, TimeSpan expiresIn, CommandFlags flags = CommandFlags.None)
        {
            var tasks = new Task<bool>[keys.Length];

            for (var i = 0; i < keys.Length; i++)
                tasks[i] = UpdateExpiryAsync(keys[i], expiresIn, flags);

            await Task.WhenAll(tasks).ConfigureAwait(false);

            var results = new Dictionary<string, bool>(keys.Length, StringComparer.Ordinal);

            for (var i = 0; i < keys.Length; i++)
                results.Add(keys[i], tasks[i].Result);

            return results;
        }
    }
}
