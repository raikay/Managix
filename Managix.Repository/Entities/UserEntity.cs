using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Managix.Repository.Entities.Base
{
    /// <summary>
    /// 用户
    /// </summary>
	[Table("ad_user")]
    public class UserEntity: Root
    {
        /// <summary>
        /// 账号
        /// </summary>
        [MaxLength(60)]
        public string UserName { get; set; }
        

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(60)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(60)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
       [MaxLength(1024)]
        public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }


        /// <summary>
        /// 编号
        /// </summary>
        [Description("权限Id")]
        public string RoleId { get; set; }

        //[Navigate(ManyToMany = typeof(UserRoleEntity))]
        //public ICollection<RoleEntity> Roles { get; set; }
    }
}
