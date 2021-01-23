using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pear.Core
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// 获取用户 Id
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        User User { get; }

        /// <summary>
        /// 检查用户是否有效
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<User> CheckUserAsync(int userId, bool tracking = true);

        /// <summary>
        /// 查询用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Security>> GetSecuritiesAsync(int userId);
    }
}