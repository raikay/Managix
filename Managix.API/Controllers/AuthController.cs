using System.Diagnostics;
using System.Threading.Tasks;
using Managix.Infrastructure;
using Managix.Infrastructure.Authentication;
using Managix.IServices;
using Managix.IServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Managix.API.Controllers
{
    /// <summary>
    /// 认证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ILogger _logger;
        private readonly ILoginLogService _loginLogService;
        private readonly IMapperService _mapService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapService"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        /// <param name="loginLogService"></param>
        public AuthController(IMapperService mapService, IAuthService service, ILogger<AuthController> logger, ILoginLogService loginLogService)
        {
            _service = service;
            _logger = logger;
            _loginLogService = loginLogService;
            _mapService = mapService;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="lastKey">上次验证码键</param>
        /// <returns></returns>
        [HttpGet("VerifyCode")]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetVerifyCode(string lastKey)
        {
            _logger.LogInformation("获取验证码");
            return await _service.GetVerifyCodeAsync(lastKey);
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        [HttpGet("PassWordEncryptKey")]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPassWordEncryptKey()
        {
            return await _service.GetPassWordEncryptKeyAsync();
        }

        /// <summary>
        /// 用户登录
        /// 根据登录信息生成Token
        /// </summary>
        /// <param name="input">登录信息</param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IResponseOutput> Login(AuthLoginParam input)
        {
            var sw = new Stopwatch();
            sw.Start();
            var res = await _service.LoginAsync(input);
            sw.Stop();

            #region 添加登录日志
            var loginLogAddInput = new LoginLogAddInput()
            {
                CreatedUserName = input.UserName,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Msg = res.Msg
            };

            ResponseOutput<AuthLoginOutput> output = null;
            if (res.Success)
            {
                output = (res as ResponseOutput<AuthLoginOutput>);
                var user = output.Data;
                loginLogAddInput.CreatedUserId = user.Id;
                loginLogAddInput.NickName = user.NickName;
            }

            await _loginLogService.AddAsync(loginLogAddInput);
            #endregion

            if (!res.Success)
            {
                return res;
            }
            var tokendto = _mapService.Map<TokenUserDto>(output.Data);
            var token = UserToken.GetToken(tokendto);
            return ResponseOutput.Ok(new { token = token });
        }


        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetUserInfo()
        {
            return await _service.GetUserInfoAsync();
        }

        /*
        /// <summary>
        /// 刷新Token
        /// 以旧换新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Refresh([BindRequired] string token)
        {
            var userClaims = _userToken.Decode(token);
            if (userClaims == null || userClaims.Length == 0)
            {
                return ResponseOutput.NotOk();
            }

            var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
            if (refreshExpires.IsNull())
            {
                return ResponseOutput.NotOk();
            }

            if (refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
            {
                return ResponseOutput.NotOk("登录信息已过期");
            }

            var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
            if (userId.IsNull())
            {
                return ResponseOutput.NotOk();
            }
            var output = await _userServices.GetLoginUserAsync(userId.ToLong());

            return GetToken(output);
        }

        */
    }
}
