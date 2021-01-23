using Furion.DatabaseAccessor;

namespace Pear.Core
{
    /// <summary>
    /// 角色和权限关系表
    /// </summary>
    public class RoleSecurity : IEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 一对一引用
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public int SecurityId { get; set; }

        /// <summary>
        /// 一对一引用
        /// </summary>
        public Security Security { get; set; }
    }
}