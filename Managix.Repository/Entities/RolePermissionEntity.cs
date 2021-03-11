using System.ComponentModel.DataAnnotations.Schema;

namespace Managix.Repository.Entities.Base
{
    /// <summary>
    /// 角色权限
    /// </summary>
	[Table("ad_role_permission")]
    public class RolePermissionEntity: Root
    {
        /// <summary>
        /// 角色Id
        /// </summary>
		public string RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
		public string PermissionId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        //public RoleEntity Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        //public PermissionEntity Permission { get; set; }
    }

}
