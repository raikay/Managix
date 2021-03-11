using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Managix.Infrastructure.Dtos;

namespace System.Linq
{
    /// <summary>
    /// Some useful extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Used for paging with an <see cref="IPagedParam"/> object.
        /// </summary>
        /// <param name="query">Queryable to apply paging</param>
        /// <param name="pagedParam">An object implements <see cref="IPagedParam"/> interface</param>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPageInput pagedParam)
        {
            var limit = pagedParam.PageSize ?? 10;
            var skipCount = pagedParam.CurrentPage > 1 ? (pagedParam.CurrentPage.Value - 1) * limit : 0;
            return query.PageBy(skipCount, limit);
        }


        /// <summary>
        /// Used for paging with an <see cref="IPagedParam"/> object.
        /// </summary>
        /// <param name="query">Queryable to apply paging</param>
        /// <param name="pagedParam">An object implements <see cref="IPagedParam"/> interface</param>
        public static IQueryable<T> Count<T>(this IQueryable<T> query, out int count)
        {
            count = query.Count();
            return query;
        }


        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<TSource> PageBy<TSource>(this IQueryable<TSource> query, int skipCount, int limitCount)
        {
            if (skipCount == 0)
            {
                return query.Take(limitCount);
            }
            return query.Skip(skipCount).Take(limitCount);
        }

        ///// <summary>
        ///// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        ///// </summary>
        //public static TQueryable PageBy<TSource, TQueryable>(this TQueryable query, int skipCount, int limitCount)
        //    where TQueryable : IQueryable<TSource>
        //{
        //    if (skipCount == 0)
        //    {
        //        return (TQueryable)query.Take(limitCount);
        //    }
        //    return (TQueryable)query.Skip(skipCount).Take(limitCount);
        //}

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<TSource> WhereIfNot<TSource>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            return condition ? query : query.Where(predicate);
        }

        ///// <summary>
        ///// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        ///// </summary>
        ///// <param name="query">Queryable to apply filtering</param>
        ///// <param name="condition">A boolean value</param>
        ///// <param name="predicate">Predicate to filter the query</param>
        ///// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        //public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
        //    where TQueryable : IQueryable<T>
        //{
        //    return condition ? (TQueryable)query.Where(predicate) : query;
        //}

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, int, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        ///// <summary>
        ///// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        ///// </summary>
        ///// <param name="query">Queryable to apply filtering</param>
        ///// <param name="condition">A boolean value</param>
        ///// <param name="predicate">Predicate to filter the query</param>
        ///// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        //public static TQueryable WhereIf<TSource, TQueryable>(this TQueryable query, bool condition, Expression<Func<TSource, int, bool>> predicate)
        //    where TQueryable : IQueryable<TSource>
        //{
        //    return condition ? (TQueryable)query.Where(predicate) : query;
        //}

        /// <summary>
        /// Project to TDestination.
        /// </summary>
        public static IQueryable<TDestination> ProjectTo<T, TDestination>(this IQueryable<T> query)
        {
            return null;//query.ProjectToType<TDestination>();
        }

        /// <summary>
        /// Project to TDestination.
        /// </summary>
        public static IQueryable<TDestination> ProjectTo<TDestination>(this IQueryable query)
        {
            return null;// query.ProjectToType<TDestination>();
        }
    }
}
