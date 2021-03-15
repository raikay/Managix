using Managix.Infrastructure.Authentication;
using Managix.IServices;
using Managix.Repository.Entities.Base;
using Managix.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Managix.Infrastructure;
using Managix.IServices.Dtos;
using Managix.Infrastructure.Dtos;

namespace Managix.Services
{
    public class UserService : Service, IUserService
    {
        private readonly ICurrentUser _user;
        private readonly IRepository<RolePermissionEntity> _rolePermissionRepo;
        private readonly IRepository<PermissionEntity> _permissionRepo;
        private readonly IRepository<UserEntity> _userRepo;
        private readonly IRepository<RoleEntity> _roleRepo;
        private readonly IMapperService _mapper;
        public UserService(IRepository<PermissionEntity> permissionRepo, IMapperService mapper, ICurrentUser user, IRepository<RolePermissionEntity> rolePermissionRepo, IRepository<UserEntity> repo, IRepository<RoleEntity> roleRepo)
        {
            _user = user;
            _rolePermissionRepo = rolePermissionRepo;
            _permissionRepo = permissionRepo;
            _roleRepo = roleRepo;
            _userRepo = repo;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取用户权限（权限校验）
        /// </summary>
        /// <returns></returns>
        public async Task<IList<UserPermissionsDto>> GetUserPermissionsAsync()
        {
            var userid = _user.Id;
            var userEntity = await _userRepo.FindByIdAsync(_user.Id);

            var listQurey = from r in _rolePermissionRepo.Query
                            join p in _permissionRepo.Query
                            on r.PermissionId equals p.Id
                            where r.RoleId==userEntity.RoleId
                            select new UserPermissionsDto { Path = p.Path, HttpMethod = p.HttpMethod };

            return await listQurey.ToListAsync();
        }



        /// <summary>
        /// 根据Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseOutput<UserGetOutput>> GetUserByIdAsync(string id)
        {
            return new ResponseOutput<UserGetOutput>().Ok(ObjectMapper.Map<UserGetOutput>(await _userRepo.FindByIdAsync(id)));
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseOutput> GetBasicAsync()
        {
            if (string.IsNullOrWhiteSpace(_user?.Id))
            {
                return ResponseOutput.NotOk("未登录！");
            }
            var data = await _userRepo.FindByIdAsync(_user.Id);
            return ResponseOutput.Ok(data);
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetUserListAsync(PageInput param)
        {
            var userQurey = _userRepo.Query.Count(out int count).PageBy(param);

            var data = await (from u in userQurey
                              join r in _roleRepo.Query
                              on u.RoleId equals r.Id into ur
                              from r2 in ur.DefaultIfEmpty()
                              select new UserListOutput
                              {
                                  Id = u.Id,
                                  CreatedTime = u.CreatedTime,
                                  Name = u.NickName,
                                  UserName = u.UserName,
                                  NickName = u.NickName,
                                  Remark = u.Remark,
                                  RoleNames = new string[] { r2.Name },
                                  Status = u.Status

                              }).ToListAsync();

            var result = new PageOutput<UserListOutput>()
            {
                List = data,
                Total = count
            };

            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> AddAsync(UserSaveInput input)
        {
            if (input.Password.IsNull())
                input.Password = "111111";
            input.Password = MD5Encrypt.Encrypt32(input.Password);
            var entity = _mapper.Map<UserEntity>(input);
            entity.CreatedTime = DateTime.Now;
            await _userRepo.InsertAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> UpdateAsync(string id, UserUpdateInput input)
        {
            if (string.IsNullOrEmpty(id))
                return ResponseOutput.NotOk();

            var user = await _userRepo.FindByIdAsync(id);

            if (string.IsNullOrWhiteSpace(id))
                return ResponseOutput.NotOk("用户不存在！");

            _mapper.Map(input, user);
            await _userRepo.UpdateAsync(user);

            return ResponseOutput.Ok();
        }






        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            await _userRepo.DeleteAsync(m => m.Id == id);

            return ResponseOutput.Ok();
        }



        /// <summary>
        /// 批量删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>

        public async Task<IResponseOutput> BatchDeleteUserAsync(List<string> ids)
        {
            await _userRepo.BulkDeleteAsync(ids);

            return ResponseOutput.Ok();
        }
    }
}
