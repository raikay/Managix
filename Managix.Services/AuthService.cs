using Managix.Infrastructure;
using Managix.Infrastructure.Authentication;
using Managix.Infrastructure.Configuration;
using Managix.Infrastructure.Helper;
using Managix.IServices;
using Managix.IServices.Dtos;
using Managix.Repository.Entities.Base;
using Managix.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Managix.Services
{
    public class AuthService : Service, IAuthService
    {
        private readonly ICache _cache;
        private readonly IRepository<UserEntity> _userRepo;
        private readonly IRepository<PermissionEntity> _permissionRepo;
        private readonly IRepository<RolePermissionEntity> _rolePermissionRepo;
        public AuthService(IRepository<RolePermissionEntity> rolePermissionRepo, IRepository<UserEntity> userRepo, ICache cache, IRepository<PermissionEntity> permissionRepo)
        {
            _userRepo = userRepo;
            _cache = cache;
            _permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
        }

        public async Task<IResponseOutput> GetPassWordEncryptKeyAsync()
        {
            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.PassWordEncryptKey, guid);
            var encyptKey = StringHelper.GenerateRandom(8);
            await _cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
            var data = new { key = guid, encyptKey };
            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> GetUserInfoAsync()
        {
            if (string.IsNullOrEmpty(User?.Id))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            //用户信息
            var user = await _userRepo.FindByIdAsync(User.Id);
            var rolePerList = _rolePermissionRepo.Query.Where(c => c.RoleId == user.RoleId);
            var rolePerIds = rolePerList.Select(x => x.PermissionId);

            //用户菜单
            var menus = await _permissionRepo.Query
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .Where(a => rolePerIds.Contains(a.Id)
                )
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .Select(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Path,
                    ViewPath = a.ViewPath,
                    a.Label,
                    a.Icon,
                    a.Opened,
                    a.Closable,
                    a.Hidden,
                    a.NewWindow,
                    a.External
                }).ToListAsync();

            //用户权限点
            var permissions = await _permissionRepo.Query
                .Where(a => new[] { PermissionType.Api, PermissionType.Dot }.Contains(a.Type))
                .Where(a => rolePerIds.Contains(a.Id)
                )
                .Select(a => a.Code).ToListAsync();
            return ResponseOutput.Ok(new { user, menus, permissions });
        }

        public async Task<IResponseOutput> GetVerifyCodeAsync(string lastKey)
        {
            var img = VerifyCodeHelper.GetBase64String(out string code);

            //删除上次缓存的验证码
            if (lastKey.NotNull())
            {
                await _cache.DelAsync(lastKey);
            }

            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.VerifyCodeKey, guid);
            await _cache.SetAsync(key, code, TimeSpan.FromMinutes(5));

            var data = new { Key = guid, Img = img };
            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> LoginAsync(AuthLoginParam param)
        {
            #region 验证码校验
            if (Configs.AppSettings.VarifyCode.Enable)
            {
                var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, param.VerifyCodeKey);
                var exists = await _cache.ExistsAsync(verifyCodeKey);
                if (exists)
                {
                    var verifyCode = await _cache.GetAsync(verifyCodeKey);
                    if (string.IsNullOrEmpty(verifyCode))
                    {
                        return ResponseOutput.NotOk("验证码已过期！");
                    }
                    if (verifyCode.ToLower() != param.VerifyCode.ToLower())
                    {
                        return ResponseOutput.NotOk("验证码输入有误！", 2);
                    }
                    await _cache.DelAsync(verifyCodeKey);
                }
                else
                {
                    return ResponseOutput.NotOk("验证码已过期！", 1);
                }
            }
            #endregion

            var user = (await _userRepo.FindAsync(a => a.UserName == param.UserName));
            if (string.IsNullOrWhiteSpace(user?.Id))
            {
                return ResponseOutput.NotOk("账号输入有误!", 3);
            }

            #region 解密
            if (param.PasswordKey.NotNull())
            {
                var passwordEncryptKey = string.Format(CacheKey.PassWordEncryptKey, param.PasswordKey);
                var existsPasswordKey = await _cache.ExistsAsync(passwordEncryptKey);
                if (existsPasswordKey)
                {
                    var secretKey = await _cache.GetAsync(passwordEncryptKey);
                    if (secretKey.IsNull())
                    {
                        return ResponseOutput.NotOk("解密失败！", 1);
                    }
                    param.Password = DesEncrypt.Decrypt(param.Password, secretKey);
                    await _cache.DelAsync(passwordEncryptKey);
                }
                else
                {
                    return ResponseOutput.NotOk("解密失败！", 1);
                }
            }
            #endregion

            var password = MD5Encrypt.Encrypt32(param.Password);
            if (user.Password != password)
            {
                return ResponseOutput.NotOk("密码输入有误！", 4);
            }

            var authLoginOutput = ObjectMapper.Map<AuthLoginOutput>(user);//user.MapTo<UserEntity, AuthLoginOutput>();

            return ResponseOutput.Ok(authLoginOutput);

        }


    }
}
