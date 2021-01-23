using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pear.Core
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User : Entity, IEntityTypeBuilder<User>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public User()
        {
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
            Enabled = true;
        }

        /// <summary>
        /// 账号
        /// </summary>
        [Required, MaxLength(32)]
        public string Account { get; set; }

        /// <summary>
        /// 密码（采用 MD5 加密）
        /// </summary>
        [Required, MaxLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(32)]
        public string Nickname { get; set; }

        /// <summary>
        /// 头像（OSS地址）
        /// </summary>
        [MaxLength(256)]
        public string Photo { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [MaxLength(200)]
        public string Synopsis { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTimeOffset SigninedTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 多对多
        /// </summary>
        public ICollection<Role> Roles { get; set; }

        /// <summary>
        /// 多对多中间表
        /// </summary>
        public List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// 配置多对多关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<User> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(p => p.Roles)
                 .WithMany(p => p.Users)
                 .UsingEntity<UserRole>(
                   u => u.HasOne(c => c.Role).WithMany(c => c.UserRoles).HasForeignKey(c => c.RoleId)
                 , u => u.HasOne(c => c.User).WithMany(c => c.UserRoles).HasForeignKey(c => c.UserId)
                 , u =>
                 {
                     u.HasKey(c => new { c.UserId, c.RoleId });
                     u.HasData(
                         new { UserId = 1, RoleId = 1 }
                     );
                 });
        }
    }
}