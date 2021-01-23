using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pear.Core
{
    /// <summary>
    /// 数据字典类别
    /// </summary>
    public class SystemDataCategory : Entity, IEntityTypeBuilder<SystemDataCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemDataCategory()
        {
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
            Sequence = 0;
            Enabled = true;
        }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required, MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 分类描述
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 序号/排序
        /// </summary>
        [Required]
        public int Sequence { get; set; }

        /// <summary>
        /// 上级分类 Id
        /// </summary>
        public int? HigherId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 上级分类
        /// </summary>
        public SystemDataCategory Higher { get; set; }

        /// <summary>
        /// 次级列表
        /// </summary>
        public ICollection<SystemDataCategory> Sublevels { get; set; }

        /// <summary>
        /// 分组数据
        /// </summary>
        public ICollection<SystemData> Data { get; set; }

        /// <summary>
        /// 配置实体
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SystemDataCategory> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder
                .HasMany(x => x.Sublevels)
                .WithOne(x => x.Higher)
                .HasForeignKey(x => x.HigherId);
        }
    }
}