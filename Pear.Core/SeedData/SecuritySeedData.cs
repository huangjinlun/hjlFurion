using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pear.Core.SeedData
{
    /// <summary>
    /// 用户表种子数据 <see cref="Security"/>
    /// </summary>
    public class SecuritySeedData : IEntitySeedData<Security>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<Security> HasData(DbContext dbContext, Type dbContextLocator)
        {
            var list = new List<Security>();

            var securities = typeof(SecurityConsts).GetFields().Select(u => u.GetRawConstantValue().ToString()).ToArray();
            for (var i = 1; i < securities.Length + 1; i++)
            {
                list.Add(new Security { Id = i, UniqueName = securities[i - 1], Remark = securities[i - 1], CreatedTime = DateTimeOffset.Parse("2020-12-22 10:00:00"), IsDeleted = false, Enabled = true });
            }

            return list;
        }
    }
}