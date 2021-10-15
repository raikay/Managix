using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Common
{
    /// <summary>
    /// 远程调用响应异常.
    /// </summary>
    public class ResponseException : Exception
    {
        private const int InvalidOperationCode = 400; // 400
        private const int ExceptionCode = 500; // 500
        private const int BadRequestCode = 400; // 400
        private const int UnauthorizedCode = 401; // 401
        private const int ForbiddenCode = 403; // 403
        private const int NotFoundCode = 404; // 404
        private const int UnprocessableEntityCode = 422; // 422
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpCode"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseException(int httpCode, int code, string message) : this(code, message)
        {
            HttpCode = httpCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseException(int code, string message) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// Http状态码
        /// </summary>
        public int HttpCode { get; private set; } = 500;

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// 返回一个400错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException BadRequest(string message = "错误的请求。") =>
            new ResponseException(400, BadRequestCode, message);

        /// <summary>
        /// 返回一个400错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException InvalidOperation(string message) => InvalidOperation(InvalidOperationCode, message);

        /// <summary>
        /// 返回一个400错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException InvalidOperation(int code, string message) =>
             new ResponseException(400, code, message);

        /// <summary>
        /// 返回一个500错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException ServerError(string message) => ServerError(ExceptionCode, message);

        /// <summary>
        /// 返回一个500错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException ServerError(int code, string message) =>
            new ResponseException(500, code, message);

        /// <summary>
        /// 返回一个401错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException Unauthorized(string message = "当前请求需要用户验证。") =>
            new ResponseException(401, UnauthorizedCode, message);

        /// <summary>
        /// 返回一个403错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException Forbidden(string message = "非法的请求。") => Forbidden(ForbiddenCode, message);

        /// <summary>
        /// 返回一个403错误
        /// </summary>
        /// <param name="code">错误编码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static ResponseException Forbidden(int code, string message) =>
            new ResponseException(403, code, message);

        /// <summary>
        /// 返回一个404错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseException NotFound(string message = "该记录未找到。") => NotFound(NotFoundCode, message);

        /// <summary>
        /// 返回一个404错误
        /// </summary>
        /// <param name="code">错误编码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static ResponseException NotFound(int code, string message) =>
            new ResponseException(404, code, message);

        /// <summary>
        /// 返回一个422错误
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static ResponseException UnprocessableEntity(string message = "请求参数格式错误。") => UnprocessableEntity(UnprocessableEntityCode, message);

        /// <summary>
        /// 返回一个422错误
        /// </summary>
        /// <param name="code">错误编码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static ResponseException UnprocessableEntity(int code, string message) =>
             new ResponseException(422, code, message);

    }
}
