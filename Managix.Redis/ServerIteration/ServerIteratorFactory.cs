using Managix.Redis.Configuration;
using StackExchange.Redis;

namespace Managix.Redis.ServerIteration
{
    /// <summary>
    /// The factory that allows you to enumerate all Redis servers.
    /// </summary>
    public static class ServerIteratorFactory
    {
        /// <summary>
        /// Rerturn all Redis servers
        /// </summary>
        /// <param name="multiplexer">The redis connection.</param>
        /// <param name="serverEnumerationStrategy">The iterate strategy.</param>
        /// <exception cref="NotImplementedException">In case of wrong enum.</exception>
        public static IEnumerable<IServer> GetServers(
            IConnectionMultiplexer multiplexer,
            ServerEnumerationStrategy serverEnumerationStrategy)
        {
            switch (serverEnumerationStrategy.Mode)
            {
                case ServerEnumerationStrategy.ModeOptions.All:
                    return new ServerEnumerable(
                                            multiplexer,
                                            serverEnumerationStrategy.TargetRole,
                                            serverEnumerationStrategy.UnreachableServerAction);

                case ServerEnumerationStrategy.ModeOptions.Single:
                    var serversSingle = new ServerEnumerable(
                                                multiplexer,
                                                serverEnumerationStrategy.TargetRole,
                                                serverEnumerationStrategy.UnreachableServerAction);
                    return serversSingle.Take(1);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
