using Managix.Infrastructure;
using Managix.IServices.Dtos;
using System;
using System.Threading.Tasks;

namespace Managix.IServices
{
    /// <summary>
    /// 权限
    /// </summary>	
    public interface IPermissionServices : IService
    {
        /// <summary>
        /// 获取权限管理列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPermissionListAsync(string key, DateTime? start, DateTime? end);
        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetUserPermissionListAsync();
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRolePermissionListAsync(string roleId = "");

        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddGroupAsync(PermissionAddGroupInput input);

        /// <summary>
        /// 获取组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetGroupAsync(string id);

        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetMenuAsync(string id);

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddMenuAsync(PermissionAddMenuInput input);

        /// <summary>
        /// 更新组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateGroupAsync(PermissionUpdateGroupInput input);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateMenuAsync(PermissionUpdateMenuInput input);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(string id);

        /// <summary>
        /// 软删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SoftDeleteAsync(string id);

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AssignAsync(PermissionAssignInput input);

        /// <summary>
        /// 获取api
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetApiAsync(string id);
        /// <summary>
        /// 添加api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddApiAsync(PermissionAddApiInput input);
        /// <summary>
        /// 更新api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateApiAsync(PermissionUpdateApiInput input);
    }
}
