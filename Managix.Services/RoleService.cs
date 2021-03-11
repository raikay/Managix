using Managix.Infrastructure;
using Managix.Infrastructure.Dtos;
using Managix.IServices;
using Managix.IServices.Dtos;
using Managix.Repository.Entities.Base;
using Managix.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managix.Services
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleService : Service, IRoleService
    {

        private readonly IRepository<RoleEntity> _roleRepo;
        public RoleService(IRepository<RoleEntity> roleRepo)
        {
            _roleRepo = roleRepo;
        }
        public async Task<IResponseOutput> GetRoleListAsync(PageInput param)
        {
            var data = await _roleRepo.Query
                 .OrderByDescending(c => c.Id)
                 .Skip((param.CurrentPage.GetValueOrDefault() - 1) * param.PageSize.GetValueOrDefault())
                 .Take(param.PageSize.GetValueOrDefault())
                 .ToListAsync();

            var resultData = ObjectMapper.MapList<RoleEntity, RoleDto>(data);
            return ResponseOutput.Ok(new Paged<RoleDto>(resultData.Count, resultData));
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetRoleDetailAsync(string id)
        {
            var data = await _roleRepo.FindByIdAsync(id);
            var resultData = ObjectMapper.Map<RoleEntity, RoleDto>(data);
            return ResponseOutput.Ok(resultData);
        }


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> AddRoleAsync(RoleSaveDto dto)
        {

            var list = await _roleRepo.FindListAsync(_ => _.Name == dto.Name);
            if (list.Count > 0)
            {
                return ResponseOutput.NotOk("名称重复");
            }
            var data = ObjectMapper.Map<RoleEntity>(dto);
            data.CreatedTime = DateTime.Now;
            await _roleRepo.InsertAsync(data);
            return ResponseOutput.Ok();
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> UpdateRoleAsync(string id, RoleSaveDto dto)
        {
            if (string.IsNullOrEmpty(id))
                return ResponseOutput.NotOk("无数据");

            var data = await _roleRepo.FindByIdAsync(id);

            if (string.IsNullOrWhiteSpace(id))
                return ResponseOutput.NotOk("用户不存在！");

            ObjectMapper.Map(dto, data);
            await _roleRepo.UpdateAsync(data);

            return ResponseOutput.Ok();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<IResponseOutput> DeleteRoleAsync(string id)
        {
            await _roleRepo.DeleteAsync(m => m.Id == id);

            return ResponseOutput.Ok();
        }


        /// <summary>
        /// 批量删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>

        public async Task<IResponseOutput> BatchDeleteRoleAsync(List<string> ids)
        {
            await _roleRepo.BulkDeleteAsync(ids);

            return ResponseOutput.Ok();
        }


    }
}
