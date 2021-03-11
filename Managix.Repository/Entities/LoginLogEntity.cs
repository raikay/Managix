using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Managix.Repository.Entities.Base
{
    /// <summary>
    /// 操作日志
    /// </summary>
	[Table("ad_login_log")]
    public class LoginLogEntity : Root
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(60)]
        public string NickName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [MaxLength(100)]
        public string IP { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [MaxLength(100)]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [MaxLength(100)]
        public string Os { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        [MaxLength(50)]
        public string Device { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 操作消息
        /// </summary>
        [MaxLength(500)]
        public string Msg { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public string Result { get; set; }
    }
}
