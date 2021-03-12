using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Configuration
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfig
    {
        public string TestName { set; get; }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType Type { get; set; } 

        /// <summary>
        /// 限流缓存类型
        /// </summary>
        public CacheType TypeRateLimit { get; set; } 

        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisConfig Redis { get; set; } 
    }

    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } 

        /// <summary>
        /// 限流连接字符串
        /// </summary>
        public string ConnectionStringRateLimit { get; set; } 
    }

    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        Memory,
        /// <summary>
        /// Redis缓存
        /// </summary>
        Redis
    }
}
