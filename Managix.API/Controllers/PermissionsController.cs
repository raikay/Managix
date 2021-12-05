using Managix.Infrastructure;
using Managix.IServices;
using Managix.IServices.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Managix.API.Controllers
{
    /// <summary>
    /// 权限
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionServices _permissionServices;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionServices"></param>
        public PermissionsController(IPermissionServices permissionServices)
        {
            _permissionServices = permissionServices;
        }

        /// <summary>
        /// 查询权限列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPermissionList(string? key, DateTime? start, DateTime? end)
        {
            return await _permissionServices.GetPermissionListAsync(key, start, end);
        }

        /// <summary>
        /// 查询角色权限-权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserPermissions")]
        public async Task<IResponseOutput> GetUserPermissions()
        {
            return await _permissionServices.GetUserPermissionListAsync();
        }

        /// <summary>
        /// 查询角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("RolePermissions")]
        public async Task<IResponseOutput> GetRolePermissionList(string roleId)
        {
            return await _permissionServices.GetRolePermissionListAsync(roleId);
        }

        #region 菜单
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Menu")]
        public Task<IResponseOutput> AddMenuAsync(IServices.Dtos.PermissionAddMenuInput dto)
        {
            return _permissionServices.AddMenuAsync(dto);
        }

        /// <summary>
        /// 查询单条菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Menu/{id}")]
        public async Task<IResponseOutput> GetMenu(string id)
        {
            return await _permissionServices.GetMenuAsync(id);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("Menu/{id}")]
        public async Task<IResponseOutput> UpdateMenu(string id, PermissionUpdateMenuInput input)
        {
            return await _permissionServices.UpdateMenuAsync(input);
        }
        #endregion

        #region 分组
        /// <summary>
        /// 查询单条分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Group/{id}")]
        public async Task<IResponseOutput> GetGroup(string id)
        {
            return await _permissionServices.GetGroupAsync(id);
        }

        /// <summary>
        /// 新增分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Group")]
        public async Task<IResponseOutput> AddGroup(PermissionAddGroupInput input)
        {
            return await _permissionServices.AddGroupAsync(input);
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("Group")]
        public async Task<IResponseOutput> UpdateGroup(PermissionUpdateGroupInput input)
        {
            return await _permissionServices.UpdateGroupAsync(input);
        }
        #endregion


        #region 接口

        /// <summary>
        /// 查询单条接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Api/{id}")]
        public async Task<IResponseOutput> GetApi(string id)
        {
            return await _permissionServices.GetApiAsync(id);
        }

        /// <summary>
        /// 新增接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Api")]
        public async Task<IResponseOutput> AddApi(PermissionAddApiInput input)
        {
            return await _permissionServices.AddApiAsync(input);
        }

        /// <summary>
        /// 修改接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("Api")]
        public async Task<IResponseOutput> UpdateApi(PermissionUpdateApiInput input)
        {
            return await _permissionServices.UpdateApiAsync(input);
        }
        #endregion

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            await _permissionServices.SoftDeleteAsync(id);
            return ResponseOutput.Ok();
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("Assign")]
        public async Task<IResponseOutput> Assign(PermissionAssignInput input)
        {
            return await _permissionServices.AssignAsync(input);
        }

























    }
}
