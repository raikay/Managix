using Managix.Infrastructure.Configuration;
using Managix.Repository.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Linq;

namespace Managix.Repository
{
    public partial class SqliteContext : DbContext
    {
        /// <summary>
        /// 自动生成DbSet
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.ExportedTypes)
            {
                if (type.IsClass && type != typeof(Root) && typeof(Root).IsAssignableFrom(type))
                {
                    var method = modelBuilder.GetType().GetMethods().Where(x => x.Name == "Entity").FirstOrDefault();

                    if (method != null)
                    {
                        method = method.MakeGenericMethod(new Type[] { type });
                        method.Invoke(modelBuilder, null);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlite(Configs.DbConfig.ConnectionString);
        }
    }
}
