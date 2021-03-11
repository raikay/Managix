using Managix.Infrastructure;
using Managix.IServices.Dtos;
using System.Threading.Tasks;

namespace Managix.IServices
{
    /// <summary>
    /// 权限服务
    /// </summary>	
    public interface IAuthService : IService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> LoginAsync(AuthLoginParam input);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetUserInfoAsync();
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="lastKey"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetVerifyCodeAsync(string lastKey);
        /// <summary>
        /// 获取密码秘钥
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetPassWordEncryptKeyAsync();
    }
}
