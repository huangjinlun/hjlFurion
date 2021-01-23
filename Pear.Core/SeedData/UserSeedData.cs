using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Pear.Core.SeedData
{
    /// <summary>
    /// 用户表种子数据 <see cref="User"/>
    /// </summary>
    public class UserSeedData : IEntitySeedData<User>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<User> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new User
                {
                    Id=1,Account="admin",Password="21232f297a57a5a743894a0e4a801fc3",CreatedTime=DateTimeOffset.Parse("2020-12-17 10:00:00"),IsDeleted=false,Enabled=true
                }
            };
        }
    }
}