using Managix.Infrastructure;
using Managix.Infrastructure.Dtos;
using Managix.IServices.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managix.IServices
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface IUserService : IService
    {
        /// <summary>
        /// 获取用户权限（权限校验）
        /// </summary>
        /// <returns></returns>
        Task<IList<UserPermissionsDto>> GetUserPermissionsAsync();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseOutput<UserGetOutput>> GetUserByIdAsync(string id);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserListAsync(PageInput input);
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(UserSaveInput input);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAsync(string id, UserUpdateInput input);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(string id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchDeleteUserAsync(List<string> ids);
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetBasicAsync();

    }
}
