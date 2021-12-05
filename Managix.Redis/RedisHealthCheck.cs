using Managix.Redis.Abstractions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Managix.Redis
{
    internal class RedisHealthCheck : IHealthCheck
    {
        private readonly IRedisConnectionPoolManager _redisConnectionPool;

        public RedisHealthCheck(IRedisConnectionPoolManager redisConnectionPool)
        {
            _redisConnectionPool = redisConnectionPool;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _redisConnectionPool.GetConnection().GetDatabase().PingAsync();
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, description: ex.Message, exception: ex);
            }
        }
    }
}
