using Managix.Infrastructure.Configuration;

namespace Managix.Infrastructure.Authentication
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessor"></param>
        public CurrentUser(Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            if (accessor.HttpContext != null)
            {
                string userIdConfig = Configs.AppSettings.IdentityServer.Enable ? ClaimAttributes.IdentityServerUserId : ClaimAttributes.UserId;
                var _user = accessor.HttpContext.User;
                var userId = _user.FindFirst(userIdConfig)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    Id = userId;
                    Name = _user.FindFirst(ClaimAttributes.UserName)?.Value;
                    NickName = _user.FindFirst(ClaimAttributes.UserNickName)?.Value;
                }

            }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { set; get; }
    }
}
