using Furion.DatabaseAccessor;

namespace Pear.Core
{
    /// <summary>
    /// 用户和角色关系表
    /// </summary>
    public class UserRole : IEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 一对一引用
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 一对一引用
        /// </summary>
        public Role Role { get; set; }
    }
}