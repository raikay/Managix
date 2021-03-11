using Managix.Infrastructure;
using Managix.Infrastructure.Authentication;
using Managix.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Managix.Services
{
    public abstract class Service : IService
    {
        public static IServiceProvider BaseServiceProvider { set; get; }

        /// <summary>
        ///  当前登录用户
        /// </summary>
        protected IUser User => BaseServiceProvider.GetRequiredService<IUser>();
        /// <summary>
        /// 对象映射器
        /// </summary>
        protected IMapperService ObjectMapper => BaseServiceProvider.GetRequiredService<IMapperService>();
        /// <summary>
        ///  Logger
        /// </summary>
        protected ILogger BaseLogger => BaseServiceProvider.GetRequiredService<ILogger>();
        /// <summary>
        ///  Cache
        /// </summary>
        protected ICache BaseCache => BaseServiceProvider.GetRequiredService<ICache>();



    }
}
