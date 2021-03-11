using Managix.Repository.Entities.Base;
using Managix.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Managix.Repository.Implement
{
    public class Repository<T> : IRepository<T> where T : Root, new()
    {
        #region 成员及构造
        public SqliteContext Db { get; set; }
        public DbSet<T> DbSet { set; get; }

        private readonly string _tableName;
        public Repository(SqliteContext context)
        {
            Db = context;
            DbSet = Db.Set<T>();
            _tableName = Db.Model.FindEntityType(typeof(T)).FindAnnotation("Relational:TableName").Value.ToString();
        }
        #endregion


        #region 查询
        IQueryable<T> IRepository<T>.Query
        {
            get { return DbSet.AsNoTracking(); }
        }

        public Task<T> FindByIdAsync(string id)
        {
            return DbSet.IgnoreQueryFilters().Where(_ => _.Id == id).SingleOrDefaultAsync();
        }

        public Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.IgnoreQueryFilters().Where(predicate).SingleOrDefaultAsync();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Task<List<T>> FindAllAsync()
        {
            return DbSet.AsNoTracking().ToListAsync();
        }
        #endregion

        #region 新增
        public async Task InsertAsync(T entity, bool autoSave = true)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Infrastructure.Helper.SnowflakeId.CreateInstance().NextId();
            }
            if (entity.CreatedTime == null)
            {
                entity.CreatedTime = DateTime.Now;
            }

            await DbSet.AddAsync(entity);
            if (autoSave)
            {
                await Db.SaveChangesAsync();
            }
        }
        public async Task BulkInsertAsync(IEnumerable<T> entities, bool autoSave = true)
        {

            await DbSet.AddRangeAsync(entities);
            if (autoSave)
            {
                await Db.SaveChangesAsync();
            }
        }
        #endregion

        #region 修改
        public async Task UpdateAsync(T entity, bool autoSave = true)
        {
            DbSet.Attach(entity);
            if (autoSave)
            {
                await Db.SaveChangesAsync();
            }
        }

        public async Task BulkUpdateAsync(IEnumerable<long> ids, Dictionary<string, object> props)
        {
            StringBuilder sqlBuilder = new StringBuilder("UPDATE ");
            sqlBuilder.Append(_tableName);
            sqlBuilder.Append(" Set ");
            var index = 0;
            foreach (var prop in props)
            {
                if (index != 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append(prop.Key);
                //1--------------------------
                //sqlBuilder.Append("= '{");
                //sqlBuilder.Append(index);
                //sqlBuilder.Append("}'");
                //2---------------------------
                sqlBuilder.Append("= '");
                sqlBuilder.Append(prop.Value);
                sqlBuilder.Append("'");
                //end---------------------------
                index++;
            }
            sqlBuilder.Append(" WHERE Id IN ('");
            sqlBuilder.Append(string.Join("','", ids));
            sqlBuilder.Append("')");
            props.Add(string.Empty, string.Empty);
            await ExecuteSqlAsync(sqlBuilder.ToString());
        }
        public async Task BulkUpdateAsync(IEnumerable<T> entities, bool autoSave = true)
        {
            DbSet.UpdateRange(entities);
            if (autoSave)
            {
                await Db.SaveChangesAsync();
            }
        }
        #endregion

        #region 删除
        public async Task DeleteAsync(T entity, bool autoSave = true)
        {
            DbSet.Remove(entity);
            if (autoSave)
            {
                await Db.SaveChangesAsync();
            }
        }

        public async Task BulkDeleteAsync(IEnumerable<string> ids)
        {
            StringBuilder sqlBuilder = new StringBuilder("DELETE FROM ");
            sqlBuilder.Append(_tableName);
            sqlBuilder.Append(" WHERE Id IN ('");
            sqlBuilder.Append(string.Join("','", ids));
            sqlBuilder.Append("')");
            await ExecuteSqlAsync(sqlBuilder.ToString());
        }

        public async Task BulkDeleteAsync(IEnumerable<T> entities, bool autoSave = true)
        {
            DbSet.RemoveRange(entities);
            if (autoSave)
            {
                await Db.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, bool autoSave = true)
        {
            List<T> entities = await DbSet.AsNoTracking().Where(predicate).ToListAsync();

            DbSet.RemoveRange(entities);

            if (autoSave)
            {
                await Db.SaveChangesAsync();
            }
        }
        #endregion

        #region  SQL操作
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task ExecuteSqlAsync(string sql)
        {
            await Db.Database.ExecuteSqlRawAsync(sql);

        }

        /// <summary>
        /// 执行带参数SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task ExecuteSqlAsync(string sql, params object[] parameters)
        {
            await Db.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        #endregion

        #region 共用
        public async Task SaveChangesAsync()
        {
            await Db.SaveChangesAsync();
        }

        #endregion

    }
}
