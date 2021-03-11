using Managix.Infrastructure.Configuration;

namespace Managix.Infrastructure.Authentication
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User : IUser
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _accessor;

        public User(Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual string Id
        {
            get
            {
                string userId = Configs.AppSettings.IdentityServer.Enable ? ClaimAttributes.IdentityServerUserId : ClaimAttributes.UserId;

                var id = _accessor?.HttpContext?.User?.FindFirst(userId);
                if (id != null && id.Value.NotNull())
                {
                    return id.Value;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get
            {
                var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserName);

                if (name != null && name.Value.NotNull())
                {
                    return name.Value;
                }

                return "";
            }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName
        {
            get
            {
                var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserNickName);

                if (name != null && name.Value.NotNull())
                {
                    return name.Value;
                }

                return "";
            }
        }
    }
}
