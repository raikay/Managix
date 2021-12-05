using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Managix.Common
{

    public interface IManagixModule
    {
        //IJAPXModule Current { get; }

        int Order { get; }

        IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
