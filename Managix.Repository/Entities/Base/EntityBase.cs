using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Managix.Repository.Entities.Base
{

    /// <summary>
    /// 实体审计
    /// </summary>
    public class EntityBase :Root 
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Description("版本")]
        public long Version { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        public long? ModifiedUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Description("修改者")]
        public string ModifiedUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        public DateTime? ModifiedTime { get; set; }


    }
}
