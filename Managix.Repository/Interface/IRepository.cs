using Managix.Repository.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Managix.Repository.Interface
{

    public interface IRepository<T> where T : Root, new()
    {
        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        IQueryable<T> Query { get; }

        Task<T> FindByIdAsync(string id);

        /// <summary>
        /// 根据条件查询一个实体
        /// 多个满足条件抛出异常
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 按条件查询类表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<T>> FindAllAsync();
        #endregion

        #region 新增
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task InsertAsync(T entity, bool autoSave = true);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task BulkInsertAsync(IEnumerable<T> entities, bool autoSave = true);
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity, bool autoSave = true);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task BulkUpdateAsync(IEnumerable<T> entities, bool autoSave = true);
        /// <summary>
        /// 根据Id批量更新
        /// </summary>
        /// <param name="ids">要更新的Id集合</param>
        /// <param name="props">要更新的字段</param>
        /// <returns></returns>
        Task BulkUpdateAsync(IEnumerable<long> ids, Dictionary<string, object> props);
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity, bool autoSave = true);

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<T, bool>> predicate, bool autoSave = true);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task BulkDeleteAsync(IEnumerable<string> ids);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task BulkDeleteAsync(IEnumerable<T> entities, bool autoSave = true);

        #endregion

        #region SQL操作
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task ExecuteSqlAsync(string sql);
        /// <summary>
        /// 执行带参数SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task ExecuteSqlAsync(string sql, params object[] parameters);
        #endregion

        #region 共用
        Task SaveChangesAsync();
        #endregion
    }
}
