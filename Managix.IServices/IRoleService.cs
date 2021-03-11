using Managix.Infrastructure;
using Managix.Infrastructure.Dtos;
using Managix.IServices.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managix.IServices
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface IRoleService : IService
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRoleListAsync(PageInput param);

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRoleDetailAsync(string id);


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddRoleAsync(RoleSaveDto dto);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateRoleAsync(string id, RoleSaveDto dto);


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<IResponseOutput> DeleteRoleAsync(string id);


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchDeleteRoleAsync(List<string> ids);
    }
}
