using Managix.Redis.Abstractions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Managix.Redis.Models
{
    /// <summary>
    /// Function in order to retrieve appropriate instance of the <see cref="IStateAwareConnection"/>
    /// </summary>
    /// <param name="connectionMultiplexer"><see cref="IConnectionMultiplexer"/> to wrap</param>
    /// <param name="logger">Optional logger</param>
    /// <returns>Appropriate instance of <see cref="IStateAwareConnection"/></returns>
    public delegate IStateAwareConnection StateAwareConnectionResolver(IConnectionMultiplexer connectionMultiplexer, ILogger logger);
}
