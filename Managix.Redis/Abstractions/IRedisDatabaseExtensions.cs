using Microsoft.Extensions.Logging;

namespace Managix.Redis.Abstractions
{
    /// <summary>
    /// The Redis Database
    /// </summary>
    public partial interface IRedisDatabase
    {
        ILogger Logger { get; }

        async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory)
        {
            var value = await GetWithCatchAsync(key, factory);
            if (!EqualityComparer<T>.Default.Equals(value, default))
            {
                return value;
            }

            // TODO  redis lock
            // Begin Lock
            //value = await GetWithCatchAsync(key, factory);
            //if (value != null)
            //{
            //    return value;
            //}

            value = await factory();
            await AddAsync(key, value);
            // End lock

            return value;
        }

        async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, DateTimeOffset expiresAt)
        {
            var value = await GetWithCatchAsync(key, factory);
            if (!EqualityComparer<T>.Default.Equals(value, default))
            {
                return value;
            }

            // TODO  redis lock
            // Begin Lock
            //value = await GetWithCatchAsync(key, factory);
            //if (value != null)
            //{
            //    return value;
            //}

            value = await factory();
            await AddAsync(key, value, expiresAt);
            // End lock

            return value;
        }

        async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiresIn)
        {
            var value = await GetWithCatchAsync(key, factory);
            if (!EqualityComparer<T>.Default.Equals(value, default))
            {
                return value;
            }

            // TODO  redis lock
            // Begin Lock
            //value = await GetWithCatchAsync(key, factory);
            //if (value != null)
            //{
            //    return value;
            //}

            value = await factory();
            await AddAsync(key, value, expiresIn);
            // End lock

            return value;
        }

        private async Task<T> GetWithCatchAsync<T>(string key, Func<Task<T>> factory)
        {
            try
            {
                return await GetAsync<T>(key);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.ToString());
                return await factory();
            }
        }
    }
}
