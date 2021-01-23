namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleProfile
    {
        /// <summary>
        /// 角色 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}