using Managix.Redis.Abstractions;
using Managix.Redis.Configuration;
using Managix.Redis.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Managix.Redis
{
    internal class StackExchangeRedisModule : Common.ManagixModule
    {
        protected override void ConfigureServicesCore(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRedisClient, RedisClient>();
            //services.AddSingleton<IRedisConnectionPoolManager, RedisSingleConnectionPoolManager>();
            services.AddSingleton<IRedisConnectionPoolManager, RedisConnectionPoolManager>();

            services.AddSingleton((provider) =>
            {
                return provider.GetRequiredService<IRedisClient>().GetDbFromConfiguration();
            });

            var options = configuration.GetSection("Redis").Get<RedisOptions>();
            if (options == null)
            {
                //throw new Exception("未配置Redis->[Redis:ConnectionString]");
            }
            services.AddSingleton(options);
        }
    }
}
