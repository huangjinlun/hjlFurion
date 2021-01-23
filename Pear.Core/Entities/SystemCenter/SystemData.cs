using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pear.Core
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class SystemData : Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemData()
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
        /// 所属分类Id
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public SystemDataCategory Category { get; set; }
    }
}