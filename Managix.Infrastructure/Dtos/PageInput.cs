using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Dtos
{
    /// <summary>
    /// 分页信息输入
    /// </summary>
    public class PageInput<T> : PageInput
    {

        /// <summary>
        /// 查询条件
        /// </summary>
        public T Filter { get; set; }

    }

    public class PageInput:IPageInput
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int? CurrentPage { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int? PageSize { set; get; } = 10;

    }

    public interface IPageInput
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int? CurrentPage { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int? PageSize { set; get; }

    }
}
