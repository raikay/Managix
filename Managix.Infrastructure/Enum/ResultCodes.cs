using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Managix.Infrastructure
{
    /// <summary>
    /// 状态码枚举
    /// </summary>
    public enum ResultCodes
    {
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Error = 0,

        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 1,

        /// <summary>
        /// 未登录（需要重新登录）
        /// </summary>
        [Description("未登录")]
        Unauthorized = 401,

        /// <summary>
        /// 权限不足
        /// </summary>
        [Description("权限不足")]
        Forbidden = 403,

        /// <summary>
        /// 资源不存在
        /// </summary>
        [Description("资源不存在")]
        NotFound = 404,

        /// <summary>
        /// 系统内部错误（非业务代码里显式抛出的异常，例如由于数据不正确导致空指针异常、数据库异常等等）
        /// </summary>
        [Description("系统内部错误")]
        ServerError = 500
    }
}
