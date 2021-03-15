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
        /// CreateScope()创建请求内唯一，否则报错 不可以从 root Provider 恢复服务
        protected ICurrentUser User => BaseServiceProvider.CreateScope().ServiceProvider.GetRequiredService<ICurrentUser>();
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
