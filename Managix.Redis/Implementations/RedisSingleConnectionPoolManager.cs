using Managix.Redis.Abstractions;
using Managix.Redis.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Managix.Redis.Implementations
{
    internal class RedisSingleConnectionPoolManager : IRedisConnectionPoolManager
    {
        private IConnectionMultiplexer _multiplexer;

        private readonly RedisOptions _redisConfiguration;
        private readonly ILogger<RedisSingleConnectionPoolManager> _logger;
        readonly object _sync_root = new();

        public RedisSingleConnectionPoolManager(RedisOptions redisConfiguration, ILogger<RedisSingleConnectionPoolManager> logger)
        {
            _redisConfiguration = redisConfiguration;
            _logger = logger;
        }

        public IConnectionMultiplexer GetConnection()
        {
            EmitConnection();
            return _multiplexer;
        }

        private void EmitConnection()
        {
            if (_multiplexer == null)
            {
                lock (_sync_root)
                {
                    if (_multiplexer == null)
                    {
                        _logger.LogInformation("Creating new Redis connection.");
                        _multiplexer = ConnectionMultiplexer.Connect(_redisConfiguration.GetConfigurationOptions());
                        //_multiplexer.ConnectionFailed += ConnectionFailed;
                        //_multiplexer.ConnectionRestored += ConnectionRestored;
                    }
                }
            }
        }

        //private void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        //{
        //    switch (e.FailureType)
        //    {
        //        case ConnectionFailureType.ConnectionDisposed:
        //        case ConnectionFailureType.InternalFailure:
        //        case ConnectionFailureType.SocketClosed:
        //        case ConnectionFailureType.SocketFailure:
        //        case ConnectionFailureType.UnableToConnect:
        //            {
        //                _logger.LogError(e.Exception, "Redis connection error {failureType}.", e.FailureType);
        //                ReInit();
        //                break;
        //            }
        //    }
        //}

        //private void ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        //{
        //    _logger.LogError(e.Exception, "Redis automatically reconnect on error {failureType}.", e.FailureType);

        //    switch (e.FailureType)
        //    {
        //        case ConnectionFailureType.None:
        //            {
        //                _logger.LogInformation("Redis connection restored successfully.");
        //                break;
        //            }
        //        default:
        //            {
        //                _logger.LogError(e.Exception, "An error connection during reconnecting `{failureType}`. Disposing the connection.", e.FailureType);
        //                ReInit();
        //                break;
        //            }
        //    }
        //}

        //public void ReInit()
        //{
        //    var oldMultiplexer = _multiplexer;
        //    _multiplexer = null;
        //    if (oldMultiplexer != null)
        //    {
        //        _logger.LogInformation("Dispose old redis connection...");
        //        oldMultiplexer.ConnectionFailed -= ConnectionFailed;
        //        oldMultiplexer.ConnectionRestored -= ConnectionRestored;
        //        oldMultiplexer.Dispose();
        //        oldMultiplexer = null;
        //    }
        //    EmitConnection();
        //}

        public void Dispose()
        {
            _logger.LogInformation("Dispose redis connection...");
            if (_multiplexer != null)
            {
                //_multiplexer.ConnectionFailed -= ConnectionFailed;
                //_multiplexer.ConnectionRestored -= ConnectionRestored;
                _multiplexer.Dispose();
                _multiplexer = null;
            }
        }
    }
}
