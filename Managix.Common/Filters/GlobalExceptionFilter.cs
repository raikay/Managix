using Managix.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Managix.Infrastructure.Filters
{
    /// <summary>
    /// 全局异常
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var message = context.Exception.Message;

            //if (context.Exception.GetType() == typeof(DiyException))
            //{
            //    //如果是自定义异常\
            //}
            //日志入库
            //向负责人发报警邮件，异步
            //向负责人发送报警短信或者报警电话，异步
            //这里获取服务器ip时，需要考虑如果是使用nginx做了负载，这里要兼容负载后的ip，
            //监控了ip方便定位到底是那台服务器出故障了
            //string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            //message = $"IP:{ip}{message}";
            _logger.LogError(context.Exception, message);

            message += $"|{context.Exception.StackTrace}";
            context.Result = new InternalServerErrorResult( ResponseOutput.NotOk(message));
            //if (context.Exception is ValidationException vex)
            //{
            //    context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            //    context.Result = new ObjectResult(new AjaxExceptionResponse { Code = vex.Code, Message = vex.Message, Errors = vex.ValidationErrors });
            //}
            //else if (context.Exception is ResponseException ex)
            //{
            //    ProcessResponseException(ex, context);
            //}
            if (_env.IsProduction())
            {
                //context.Result = new InternalServerErrorResult(new ResultData { Msg = message, Code = 0 });
                //标记异常已处理
                context.ExceptionHandled = true;
            }


            /*
             
        /// <summary>
        /// 返回一个400错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static JAPXResponseException BadRequest(string message = "错误的请求。") =>
            new(400, BadRequestCode, message);
             */
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class InternalServerErrorResult : ObjectResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public InternalServerErrorResult(object value) : base(value)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
        }
    }
}
