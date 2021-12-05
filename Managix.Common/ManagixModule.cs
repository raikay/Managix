using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Managix.Common
{
    public class ManagixModule: IManagixModule
    {
        private bool _isInint;

        public virtual int Order { get; }

        public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Ensure init once
            if (_isInint)
            {
                return services;
            }

            ConfigureServicesCore(services, configuration);

            _isInint = true;

            return services;
        }

        protected virtual void ConfigureServicesCore(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureServicesCore(services);
        }

        protected virtual void ConfigureServicesCore(IServiceCollection services)
        {

        }
    }
}
