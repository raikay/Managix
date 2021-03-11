using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Managix.Infrastructure.Configuration
{
    public class Configs
    {
        private static IConfiguration _configuration;
        public static void Init(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// AppSettings
        /// </summary>
        public static AppSettings AppSettings => _configuration.Get<AppSettings>();
        /// <summary>
        /// 跨域地址
        /// </summary>
        public static List<string> CorUrls => AppSettings.CorUrls;
        /// <summary>
        /// 缓存配置
        /// </summary>
        public static CacheConfig CacheConfig => AppSettings.CacheConfig;
        /// <summary>
        /// Db配置
        /// </summary>
        public static DbConfig DbConfig => AppSettings.DbConfig;
        /// <summary>
        /// Jwt配置
        /// </summary>
        public static JwtConfig JwtConfig => AppSettings.JwtConfig;

        /// <summary>
        /// 操作日志
        /// </summary>
        public static string LogOperation => _configuration.GetValue<string>("log:operation");

        /// <summary>
        /// 上传文件配置
        /// </summary>
        public static UploadConfig UploadConfig => AppSettings.UploadConfig;

    }


}
