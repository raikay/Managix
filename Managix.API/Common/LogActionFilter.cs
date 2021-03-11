using Managix.API.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Managix.API.Filters
{
    /// <summary>
    /// 处理日志
    /// </summary>
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogger _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 处理日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(OprationLogAttribute)))
            {
                var sw = new Stopwatch();
                sw.Start();
                var actionExecutedContext = await next();
                sw.Stop();
                var actionResult = actionExecutedContext.Result;
                //操作参数
                var args = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments);
                //操作结果
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(actionResult);

                try
                {
                    var input = new
                    {
                        ApiMethod = context.HttpContext.Request.Method.ToLower(),
                        ApiPath = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
                        ElapsedMilliseconds = sw.ElapsedMilliseconds
                    };
                    var inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(input);

                    using (_logger.BeginScope(Guid.NewGuid()))
                    {
                        _logger.LogInformation($"操作参数: \r\n{args}");
                        _logger.LogInformation($"操作结果: \r\n{result}");
                        _logger.LogInformation($"请求地址: \r\n{input}");
                    }
                    //插入数据库
                    //await _oprationLogService.AddAsync(input);
                }
                catch (Exception ex)
                {
                    _logger.LogError("操作日志插入异常：{@ex}", ex);
                }

            }
            else
            {
                await next();
            }
        }
    }
}
