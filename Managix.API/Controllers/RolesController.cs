using Managix.Infrastructure;
using Managix.Infrastructure.Dtos;
using Managix.IServices;
using Managix.IServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managix.API.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleService"></param>
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 查询分页角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput model)
        {
            return await _roleService.GetRoleListAsync(model);
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IResponseOutput> GetDetailAsync(string id)
        {
            return await _roleService.GetRoleDetailAsync(id);
        }


        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IResponseOutput> UpdateRoleAsync(string id, RoleSaveDto dto)
        {
            return await _roleService.UpdateRoleAsync(id, dto);
        }


        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddRoleAsync(RoleSaveDto dto)
        {
            return await _roleService.AddRoleAsync(dto);
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IResponseOutput> DeleteRoleAsync(string id)
        {
            return await _roleService.DeleteRoleAsync(id);
        }

        /// <summary>
        /// 批量删除角色 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("batch")]
        [AllowAnonymous]
        public async Task<IResponseOutput> BatchDeleteRoleAsync(List<string> ids)
        {
            return await _roleService.BatchDeleteRoleAsync(ids);
        }

    }
}
