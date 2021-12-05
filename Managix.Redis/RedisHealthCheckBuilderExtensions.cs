using Managix.Redis;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder builder, string name = "redis", HealthStatus? failureStatus = null, IEnumerable<string> tags = null, TimeSpan? timeout = null)
        {
            return builder.AddCheck<RedisHealthCheck>(name, failureStatus, tags, timeout);
        }
    }
}
