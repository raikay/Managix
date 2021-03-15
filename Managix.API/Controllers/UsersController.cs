using Managix.API.Attributes;
using Managix.Infrastructure;
using Managix.Infrastructure.Authentication;
using Managix.Infrastructure.Configuration;
using Managix.Infrastructure.Dtos;
using Managix.Infrastructure.Helper;
using Managix.IServices;
using Managix.IServices.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managix.API.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 查询分页角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetUserListAsync([FromQuery] PageInput param)
        {
            return await _userService.GetUserListAsync(param);
        }

        /// <summary>
        /// 根据Id查询用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OprationLog]
        public async Task<IResponseOutput> GetUserAsync(string id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        /// <summary>
        /// 获取个人基本信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("getbasic")]
        public async Task<IResponseOutput> GetBasicAsync()
        {
            return await _userService.GetBasicAsync();
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddAsync(UserSaveInput dto)
        {
            return await _userService.AddAsync(dto);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            return await _userService.DeleteAsync(id);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IResponseOutput> UpdateAsync(string id, UserUpdateInput input)
        {
            return await _userService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("changePassword")]
        public async Task<IResponseOutput> ChangePasswordAsync(UserChangePasswordInput input)
        {
            return ResponseOutput.Ok();
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="_user"></param>
        /// <param name="_uploadHelper"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("uploadAvatar")]
        public async Task<IResponseOutput> UploadAvatarAsync([FromServices] ICurrentUser _user, [FromServices] UploadHelper _uploadHelper, [FromForm] IFormFile file)
        {
            var config = Configs.UploadConfig.Avatar;
            var res = await _uploadHelper.UploadAsync(file, config, new { _user.Id });
            if (res.Success)
            {
                return ResponseOutput.Ok(res.Data.FileRelativePath);
            }

            return ResponseOutput.NotOk("上传失败！");
        }

        /// <summary>
        /// 批量删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("batch")]
        public async Task<IResponseOutput> BatchDeleteUserAsync(List<string> ids)
        {
            await _userService.BatchDeleteUserAsync(ids);

            return ResponseOutput.Ok();
        }

    }
}
