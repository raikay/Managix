using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Configuration
{
    public class AppSettings
    {
        public string TestName { set; get; }

        /// <summary>
        /// 跨域地址
        /// </summary>
        public List<string> CorUrls { set; get; }
        /// <summary>
        /// 缓存配置
        /// </summary>
        public CacheConfig CacheConfig { set; get; }
        /// <summary>
        /// Db配置
        /// </summary>
        public DbConfig DbConfig { set; get; }
        /// <summary>
        /// Jwt配置
        /// </summary>
        public JwtConfig JwtConfig { set; get; }
        /// <summary>
        /// 验证码
        /// </summary>
        public VarifyCode VarifyCode { set; get; }

        /// <summary>
        /// 上传文件配置
        /// </summary>
        public UploadConfig UploadConfig { set; get; }
        /// <summary>
        ///  统一认证授权服务器配置
        /// </summary>
        public IdentityServer IdentityServer { set; get; }

    }



    /// <summary>
    /// 验证码
    /// </summary>
    public class VarifyCode
    {
        public bool Enable { set; get; }
        public List<string> Fonts { set; get; }
    }
    /// <summary>
    /// 统一认证授权服务器配置
    /// </summary>
    public class IdentityServer
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; } = false;
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; } = "https://localhost:5000";
    }


}
