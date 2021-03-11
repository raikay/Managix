using Managix.Infrastructure;
using Managix.Infrastructure.Dtos;
using Managix.Infrastructure.Extensions;
using Managix.Infrastructure.Helper;
using Managix.IServices;
using Managix.IServices.Dtos;
using Managix.Repository.Entities.Base;
using Managix.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Managix.Services
{
    public class LoginLogService : Service, ILoginLogService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IRepository<LoginLogEntity> _loginLogRepository;
        public LoginLogService(
            IHttpContextAccessor context,
            IRepository<LoginLogEntity> loginLogRepository
        )
        {
            _context = context;
            _loginLogRepository = loginLogRepository;
        }

        public async Task<IResponseOutput> PageAsync(PageInput<LoginLogEntity> input)
        {
            var userName = input.Filter?.CreatedUserName;

            var list = _loginLogRepository.Query
            .Where(a => a.CreatedUserName.Contains(userName))
            .OrderByDescending(c => c.Id)
            .Skip((input.CurrentPage.GetValueOrDefault() - 1) * input.PageSize.GetValueOrDefault()).Take(input.PageSize.GetValueOrDefault());



            var data = new PageOutput<LoginLogListOutput>()
            {
                List = ObjectMapper.MapList<LoginLogEntity, LoginLogListOutput>(list.ToList()), //list.ToList().MapTo<List<LoginLogEntity>, List<LoginLogListOutput>>(),
                Total = list.Count()
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> AddAsync(LoginLogAddInput input)
        {
            input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);

            string ua = _context.HttpContext.Request.Headers["User-Agent"];
            if (ua.NotNull())
            {
                var client = UAParser.Parser.GetDefault().Parse(ua);
                var device = client.Device.Family;
                device = device.ToLower() == "other" ? "" : device;
                input.Browser = client.UA.Family;
                input.Os = client.OS.Family;
                input.Device = device;
                input.BrowserInfo = ua;
            }
            var entity = ObjectMapper.Map<LoginLogEntity>(input);
            await _loginLogRepository.InsertAsync(entity);

            return ResponseOutput.Ok();
        }
    }
}
