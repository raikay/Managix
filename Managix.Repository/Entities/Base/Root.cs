using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Managix.Repository.Entities.Base
{
    /// <summary>
    /// 实体创建审计
    /// </summary>
    public class Root
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public  string Id { get; set; }
        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        public string CreatedUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Description("创建者")]
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime? CreatedTime { get; set; }
    }

}
