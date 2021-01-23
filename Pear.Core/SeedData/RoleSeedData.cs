using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Pear.Core.SeedData
{
    /// <summary>
    /// 用户表种子数据 <see cref="Role"/>
    /// </summary>
    public class RoleSeedData : IEntitySeedData<Role>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<Role> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new Role
                {
                    Id=1,Name="超级管理员",Remark="拥有所有权限",CreatedTime=DateTimeOffset.Parse("2020-12-17 10:00:00"),IsDeleted=false,Enabled=true
                },
                new Role
                {
                    Id=2,Name="测试用户",Remark="只有测试权限",CreatedTime=DateTimeOffset.Parse("2020-12-17 10:00:00"),IsDeleted=false,Enabled=true
                }
            };
        }
    }
}