using Managix.Infrastructure;
using Managix.IServices;
using Managix.IServices.Dtos;
using Managix.Repository.Entities.Base;
using Managix.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Managix.Infrastructure.Helper;

namespace Managix.Services
{


    public class PermissionServices : Service, IPermissionServices
    {
        private readonly IRepository<PermissionEntity> _permissionRepo;
        private readonly IRepository<RolePermissionEntity> _rolePermissionRepo;
        public PermissionServices(IRepository<RolePermissionEntity> rolePermissionRepo, IRepository<PermissionEntity> permissionRepo)
        {
            _permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
        }
        /// <summary>
        /// 获取权限管理列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetPermissionListAsync(string key, DateTime? start, DateTime? end)
        {
            if (end.HasValue)
                end = end.Value.AddDays(1);

            var dataQuery = _permissionRepo.Query
                .WhereIfNot(string.IsNullOrWhiteSpace(key), a => a.Path.Contains(key) || a.Label.Contains(key))
                .WhereIf(start.HasValue && end.HasValue, a => start.Value > a.CreatedTime.Value && a.CreatedTime.Value >= end.Value);

            var data = await dataQuery
               .OrderBy(a => a.ParentId).ThenBy(a => a.Sort)
               .ToListAsync();

            var resultData = ObjectMapper.MapList<PermissionEntity, PermissionListOutput>(data);

            resultData.ForEach((x) => x.ApiPath = x.Path);
            return ResponseOutput.Ok(resultData);
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseOutput> GetUserPermissionListAsync()
        {
            var permissions = await _permissionRepo.Query
                .OrderBy(a => a.ParentId)
                .ThenBy(a => a.Sort).Select(a => new { a.Id, a.ParentId, a.Label, a.Type })
                .ToListAsync();

            var apis = permissions
                .Where(a => a.Type == PermissionType.Api)
                .Select(a => new { a.Id, a.ParentId, a.Label });

            var menus = permissions
                .Where(a => (new[] { PermissionType.Group, PermissionType.Menu }).Contains(a.Type))
                .Select(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Label,
                    Apis = apis.Where(b => b.ParentId == a.Id).Select(b => new { b.Id, b.Label })
                });

            return ResponseOutput.Ok(menus);
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetRolePermissionListAsync(string roleId = "")
        {
            var permissionIds = await _rolePermissionRepo.Query
                .Where(d => d.RoleId == roleId)
                .Select(a => a.PermissionId)
                .ToListAsync();

            return ResponseOutput.Ok(permissionIds);
        }


        public async Task<IResponseOutput> GetGroupAsync(string id)
        {
            var data = await _permissionRepo.FindByIdAsync(id);
            var dto = ObjectMapper.Map<PermissionGetGroupOutput>(data);
            return ResponseOutput.Ok(dto);
        }

        public async Task<IResponseOutput> GetMenuAsync(string id)
        {
            var data = await _permissionRepo.FindByIdAsync(id);
            var dto = ObjectMapper.Map<PermissionGetMenuOutput>(data);
            return ResponseOutput.Ok(dto);
        }



        public async Task<IResponseOutput> AddGroupAsync(PermissionAddGroupInput input)
        {
            var entity = ObjectMapper.Map<PermissionEntity>(input);
            await _permissionRepo.InsertAsync(entity);
            return ResponseOutput.Ok();
        }


        /// <summary>
        /// 添加菜单 OK
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> AddMenuAsync(PermissionAddMenuInput input)
        {
            var entity = ObjectMapper.Map<PermissionEntity>(input);
            await _permissionRepo.InsertAsync(entity);
            return ResponseOutput.Ok();
        }



        public async Task<IResponseOutput> UpdateGroupAsync(PermissionUpdateGroupInput input)
        {
            if (!string.IsNullOrWhiteSpace(input?.Id))
            {
                var entity = await _permissionRepo.FindByIdAsync(input.Id);
                entity = ObjectMapper.Map(input, entity);
                await _permissionRepo.UpdateAsync(entity);
            }
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> UpdateMenuAsync(PermissionUpdateMenuInput input)
        {
            if (!string.IsNullOrWhiteSpace(input?.Id))
            {
                var entity = await _permissionRepo.FindByIdAsync(input.Id);
                entity = ObjectMapper.Map(input, entity);
                await _permissionRepo.UpdateAsync(entity);
            }
            return ResponseOutput.Ok();
        }




        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            await _permissionRepo.DeleteAsync(new PermissionEntity { Id = id });

            return ResponseOutput.Ok();
        }

        public async Task SoftDeleteAsync(string id)
        {
            await _permissionRepo.DeleteAsync(new PermissionEntity { Id = id });
        }

        public async Task<IResponseOutput> AssignAsync(PermissionAssignInput input)
        {
            //查询角色权限
            var permissionIds = await _rolePermissionRepo.Query.Where(d => d.RoleId == input.RoleId).Select(x => x.PermissionId).ToListAsync();

            //批量删除权限
            var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
            if (deleteIds.Count() > 0)
            {
                await _rolePermissionRepo.DeleteAsync(m => m.RoleId == input.RoleId && deleteIds.Contains(m.PermissionId));
            }

            //批量插入权限
            var insertRolePermissions = new List<RolePermissionEntity>();
            var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));
            if (insertPermissionIds.Count() > 0)
            {
                foreach (var permissionId in insertPermissionIds)
                {
                    var id = SnowflakeId.CreateInstance().NextId();

                    insertRolePermissions.Add(new RolePermissionEntity()
                    {
                        Id = id,
                        RoleId = input.RoleId,
                        PermissionId = permissionId,
                    });
                }
                await _rolePermissionRepo.BulkInsertAsync(insertRolePermissions);
            }


            return ResponseOutput.Ok();
        }



        public async Task<IResponseOutput> GetApiAsync(string id)
        {
            var entity = await _permissionRepo.FindByIdAsync(id); //
            var result = ObjectMapper.Map<PermissionGetApiOutput>(entity);
            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> AddApiAsync(PermissionAddApiInput input)
        {
            var entity = ObjectMapper.Map<PermissionEntity>(input);
            await _permissionRepo.InsertAsync(entity);
            return ResponseOutput.Ok();
        }
        public async Task<IResponseOutput> UpdateApiAsync(PermissionUpdateApiInput input)
        {
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var entity = await _permissionRepo.FindByIdAsync(input.Id);
                entity = ObjectMapper.Map(input, entity);
                await _permissionRepo.UpdateAsync(entity);
                return ResponseOutput.Ok();
            }
            return ResponseOutput.NotOk();
        }


    }
}
