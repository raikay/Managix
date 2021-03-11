using Managix.Repository.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Managix.IServices.Dtos
{
    /// <summary>
    /// 用户权限dto
    /// </summary>
    public class UserPermissionsDto
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string HttpMethod { get; set; }
    }


    #region Input
    /// <summary>
    /// 添加菜单dto
    /// </summary>
    public class PermissionAddMenuInput
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 视图
        /// </summary>
        public long? ViewId { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; }

        ///// <summary>
        ///// 启用
        ///// </summary>
        //public bool Enabled { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 可关闭
        /// </summary>
        public bool? Closable { get; set; }

        /// <summary>
        /// 打开新窗口
        /// </summary>
        public bool? NewWindow { get; set; }

        /// <summary>
        /// 链接外显
        /// </summary>
        public bool? External { get; set; }
    }


    /// <summary>
    /// 添加权限点 dto
    /// </summary>
    public class PermissionAddDotInput
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; } = PermissionType.Dot;

        /// <summary>
        /// 父级节点
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
    }


    #region 添加分组
    /// <summary>
    /// 添加分组 dto
    /// </summary>
    public class PermissionUpdateGroupInput : PermissionAddGroupInput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }

    /// <summary>
    /// 添加分组 dto
    /// </summary>
    public class PermissionAddGroupInput
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }

        ///// <summary>
        ///// 说明
        ///// </summary>
        //public string Description { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; }

        ///// <summary>
        ///// 启用
        ///// </summary>
        //public bool Enabled { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 打开
        /// </summary>
        public bool? Opened { get; set; }
    }


    #endregion

    public class PermissionAssignInput
    {
        [Required(ErrorMessage = "角色不能为空！")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "权限不能为空！")]
        public List<string> PermissionIds { get; set; }
    }

    public class PermissionUpdateDotInput : PermissionAddDotInput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }

    public class PermissionUpdateMenuInput : PermissionAddMenuInput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
    #endregion




    #region OutPut
    public class PermissionGetDotOutput : PermissionUpdateDotInput
    {

    }

    public class PermissionGetGroupOutput : PermissionUpdateGroupInput
    {
    }

    public class PermissionGetMenuOutput : PermissionUpdateMenuInput
    {
    }

    #endregion


    #region 接口
    public class PermissionAddApiInput
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; }

        ///// <summary>
        ///// 启用
        ///// </summary>
        //public bool Enabled { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
    }

    public class PermissionUpdateApiInput : PermissionAddApiInput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }

    public class PermissionGetApiOutput : PermissionUpdateApiInput
    {

    }
    #endregion

}
