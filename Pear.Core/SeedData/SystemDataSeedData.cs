using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Pear.Core.SeedData
{
    /// <summary>
    /// 数据字典种子数据
    /// </summary>
    public class SystemDataSeedData : IEntitySeedData<SystemData>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SystemData> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new List<SystemData>
            {
                new SystemData { Id=1,CreatedTime =DateTimeOffset.Parse("2020-12-22 15:30:20"),IsDeleted=false,Name="男",Remark="男",Sequence=0,CategoryId=1,Enabled=true },
                new SystemData { Id=2,CreatedTime =DateTimeOffset.Parse("2020-12-22 15:30:20"),IsDeleted=false,Name="女",Remark="女",Sequence=1,CategoryId=1,Enabled=true },
            };
        }
    }
}