using Managix.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Filters
{
    /// <summary>
    /// 格式化【验证消息】返回格式  
    /// </summary>
    public class ValiderActionFilter : ActionFilterAttribute//: IActionFilter
    {
        /// <summary>
        /// 执行中
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> msgList = new List<string>();
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        msgList.Add(error.ErrorMessage);
                    }
                }
                var msg = string.Join(";", msgList);
                if (msgList.Count > 1)
                {
                    msg += ";";
                }
                context.Result = new JsonResult( ResponseOutput.NotOk(msg));
                return;
            }

        }

    }
}
