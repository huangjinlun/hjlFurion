using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pear.Core
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class Security : Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Security()
        {
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
            Enabled = true;
        }

        /// <summary>
        /// 权限唯一名（每一个接口）
        /// </summary>
        [Required, MaxLength(100)]
        public string UniqueName { get; set; }

        /// <summary>
        /// 全新描述
        /// </summary>
        [Required, MaxLength(200)]
        public string Remark { get; set; }

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
        public List<RoleSecurity> RoleSecurities { get; set; }
    }
}