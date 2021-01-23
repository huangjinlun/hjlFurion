using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Pear.EntityFramework.Core
{
    [AppDbContext("Furion", DbProvider.MySql)]
    public class PearDbContext : AppDbContext<PearDbContext>, IModelBuilderFilter
    {
        public PearDbContext(DbContextOptions<PearDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 配置假删除过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void OnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            var expression = base.FakeDeleteQueryFilterExpression(entityBuilder, dbContext);
            if (expression == null) return;

            entityBuilder.HasQueryFilter(expression);
        }
    }
}