using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Managix.Repository.Entities.Base
{
    /// <summary>
    /// 角色
    /// </summary>
	[Table("ad_role")]
    public class RoleEntity: Root
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [MaxLength(200)]
		public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }

        //[Navigate(ManyToMany = typeof(UserRoleEntity))]
        //public ICollection<UserEntity> Users { get; set; }

        //[Navigate(ManyToMany = typeof(RolePermissionEntity))]
        //public ICollection<PermissionEntity> Permissions { get; set; }
    }

}
