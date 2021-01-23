using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pear.Core
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role : Entity, IEntityTypeBuilder<Role>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Role()
        {
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
            Enabled = true;
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required, MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 多对多
        /// </summary>
        public ICollection<User> Users { get; set; }

        /// <summary>
        /// 多对多中间表
        /// </summary>
        public List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// 多对多
        /// </summary>
        public ICollection<Security> Securities { get; set; }

        /// <summary>
        /// 多对多中间表
        /// </summary>
        public List<RoleSecurity> RoleSecurities { get; set; }

        /// <summary>
        /// 配置多对多关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<Role> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(p => p.Securities)
                .WithMany(p => p.Roles)
                .UsingEntity<RoleSecurity>(
                  u => u.HasOne(c => c.Security).WithMany(c => c.RoleSecurities).HasForeignKey(c => c.SecurityId)
                , u => u.HasOne(c => c.Role).WithMany(c => c.RoleSecurities).HasForeignKey(c => c.RoleId)
                , u =>
                {
                    u.HasKey(c => new { c.RoleId, c.SecurityId });
                    // 添加多对多种子数据
                    u.HasData(
                        new { RoleId = 1, SecurityId = 1 },
                        new { RoleId = 1, SecurityId = 2 },
                        new { RoleId = 1, SecurityId = 3 },
                        new { RoleId = 1, SecurityId = 4 },
                        new { RoleId = 1, SecurityId = 5 },
                        new { RoleId = 1, SecurityId = 6 },
                        new { RoleId = 1, SecurityId = 7 },
                        new { RoleId = 1, SecurityId = 8 },
                        new { RoleId = 1, SecurityId = 9 },
                        new { RoleId = 1, SecurityId = 10 },
                        new { RoleId = 1, SecurityId = 11 },
                        new { RoleId = 1, SecurityId = 12 },
                        new { RoleId = 1, SecurityId = 13 },
                        new { RoleId = 1, SecurityId = 14 },
                        new { RoleId = 1, SecurityId = 15 },
                        new { RoleId = 1, SecurityId = 16 },
                        new { RoleId = 1, SecurityId = 17 },
                        new { RoleId = 1, SecurityId = 18 },
                        new { RoleId = 1, SecurityId = 19 },
                        new { RoleId = 1, SecurityId = 20 },
                        new { RoleId = 1, SecurityId = 21 }
                    );
                });
        }
    }
}