using System.Runtime.InteropServices;
using Managix.Redis.Abstractions;
using Managix.Redis.Configuration;
using Managix.Redis.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using StackExchange.Redis;

namespace Managix.Redis.Implementations
{
    internal class RedisConnectionPoolManager : IRedisConnectionPoolManager
    {
        private IStateAwareConnection[] _connections;
        private readonly RedisOptions _redisOptions;
        private readonly ILogger<RedisConnectionPoolManager> _logger;
        private bool isDisposed;
        private bool hasInit;
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private static readonly object @lock = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionPoolManager"/> class.
        /// </summary>
        /// <param name="redisOptions">The redis configuration.</param>
        /// <param name="logger">The logger.</param>
        public RedisConnectionPoolManager(RedisOptions redisOptions, ILogger<RedisConnectionPoolManager> logger = null)
        {
            _redisOptions = redisOptions ?? throw new ArgumentNullException(nameof(redisOptions));
            _logger = logger ?? NullLogger<RedisConnectionPoolManager>.Instance;
        }

        public IConnectionMultiplexer GetConnection()
        {
            EnsureEmitConnections();
            var connection = _connections.OrderBy(x => x.TotalOutstanding()).First();

            _logger.LogDebug("Using connection {0} with {1} outstanding!", connection.Connection.GetHashCode(), connection.TotalOutstanding());

            return connection.Connection;
        }

        public ConnectionPoolInformation GetConnectionInformations()
        {
            var activeConnections = 0;
            var invalidConnections = 0;

            foreach (var connection in _connections)
            {
                if (!connection.IsConnected())
                {
                    invalidConnections++;
                    continue;
                }

                activeConnections++;
            }

            return new ConnectionPoolInformation()
            {
                RequiredPoolSize = _redisOptions.PoolSize,
                ActiveConnections = activeConnections,
                InvalidConnections = invalidConnections
            };
        }

        private void EnsureEmitConnections()
        
        {
            if (hasInit)
            {
                return;
            }
            else
            {
                lock (@lock)
                {
                    if (!hasInit)
                    {
                        _connections = new IStateAwareConnection[_redisOptions.PoolSize];
                        EmitConnections();
                        hasInit = true;
                    }
                }
            }
        }

        private void EmitConnections()
        {
            for (var i = 0; i < _redisOptions.PoolSize; i++)
            {
                var multiplexer = ConnectionMultiplexer.Connect(_redisOptions.GetConfigurationOptions());

                if (_redisOptions.ProfilingSessionProvider != null)
                    multiplexer.RegisterProfiler(_redisOptions.ProfilingSessionProvider);

                _connections[i] = _redisOptions.StateAwareConnectionFactory(multiplexer, _logger);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                // free managed resources
                foreach (var connection in _connections)
                    connection.Dispose();
            }

            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }

            isDisposed = true;
        }

        /// <summary>
        ///     Wraps a <see cref="ConnectionMultiplexer" /> instance. Subscribes to certain events of the
        ///     <see cref="ConnectionMultiplexer" /> object and invalidates it in case the connection transients into a state to be
        ///     considered as permanently disconnected.
        /// </summary>
        internal sealed class StateAwareConnection : IStateAwareConnection
        {
            private readonly ILogger logger;

            /// <summary>
            ///     Initializes a new instance of the <see cref="StateAwareConnection" /> class.
            /// </summary>
            /// <param name="multiplexer">The <see cref="ConnectionMultiplexer" /> connection object to observe.</param>
            /// <param name="logger">The logger.</param>
            public StateAwareConnection(IConnectionMultiplexer multiplexer, ILogger logger)
            {
                Connection = multiplexer ?? throw new ArgumentNullException(nameof(multiplexer));
                Connection.ConnectionFailed += ConnectionFailed;
                Connection.ConnectionRestored += ConnectionRestored;
                Connection.InternalError += InternalError;
                Connection.ErrorMessage += ErrorMessage;

                this.logger = logger;
            }

            public IConnectionMultiplexer Connection { get; }

            public long TotalOutstanding() => Connection.GetCounters().TotalOutstanding;

            public bool IsConnected() => !Connection.IsConnecting;

            public void Dispose()
            {
                Connection.ConnectionFailed -= ConnectionFailed;
                Connection.ConnectionRestored -= ConnectionRestored;
                Connection.InternalError -= InternalError;
                Connection.ErrorMessage -= ErrorMessage;

                Connection.Dispose();
            }

            private void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
            {
                logger.LogError(e.Exception, "Redis connection error {0}.", e.FailureType);
            }

            private void ConnectionRestored(object sender, ConnectionFailedEventArgs e)
            {
                logger.LogError("Redis connection error restored.");
            }

            private void InternalError(object sender, InternalErrorEventArgs e)
            {
                logger.LogError(e.Exception, "Redis internal error {0}.", e.Origin);
            }

            private void ErrorMessage(object sender, RedisErrorEventArgs e)
            {
                logger.LogError("Redis error: " + e.Message);
            }
        }
    }
}
