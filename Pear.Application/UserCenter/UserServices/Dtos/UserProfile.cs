using Pear.Core;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 头像（OSS地址）
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}