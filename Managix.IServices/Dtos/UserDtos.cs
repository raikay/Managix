using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Managix.IServices.Dtos
{


    /// <summary>
    /// 添加
    /// </summary>
    public class UserSaveInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        ///// <summary>
        ///// 头像
        ///// </summary>
        //public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public long RoleId { get; set; }
    }

    /// <summary>
    /// 修改
    /// </summary>
    public partial class UserUpdateInput
    {

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }

    public class UserGetOutput : UserUpdateInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }
    }



    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserChangePasswordInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required(ErrorMessage = "请输入旧密码")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "请输入新密码")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required(ErrorMessage = "请输入确认新密码")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }


    /// <summary>
    /// 更新基本信息
    /// </summary>
    public class UserUpdateBasicInput 
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "请输入昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }

    public class UserListOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string[] RoleNames { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }
    }
}
