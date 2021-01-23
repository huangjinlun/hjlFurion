namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 权限信息
    /// </summary>
    public class SecurityProfile
    {
        /// <summary>
        /// 权限 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 权限唯一名（每一个接口）
        /// </summary>
        public string UniqueName { get; set; }

        /// <summary>
        /// 全新描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}