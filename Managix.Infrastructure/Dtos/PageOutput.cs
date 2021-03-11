using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Dtos
{
    /// <summary>
    /// 分页信息输出
    /// </summary>
    public class PageOutput<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Total { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> List { get; set; }
    }


    /// <summary>
    /// Implements <see cref="IPaged{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="List"/>list</typeparam>
    public class Paged<T> : IPaged<T>
    {
        private IReadOnlyList<T> _items;

        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int Total { get; set; }


        /// <summary>
        /// List of items.
        /// </summary>
        public IReadOnlyList<T> List
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }

        /// <summary>
        /// Creates a new <see cref="Paged{T}"/> object.
        /// </summary>
        public Paged()
        {

        }

        /// <summary>
        /// Creates a new <see cref="Paged{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public Paged(int totalCount, IReadOnlyList<T> items)
        {
            Total = totalCount;
            List = items;
        }
    }

    /// <summary>
    /// This interface is defined to standardize to return a page of items to clients.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="List"/>list</typeparam>
    public interface IPaged<T>
    {
        /// <summary>
        /// Total count of Items.
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// List of items.
        /// </summary>
        IReadOnlyList<T> List { get; set; }
    }
}
