using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pear.Core
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public class UserManager : IUserManager, IScoped
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// HttpContext 访问器
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 缓存
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userRepository"></param>
        /// <param name="memoryCache"></param>
        public UserManager(IHttpContextAccessor httpContextAccessor
            , IRepository<User> userRepository
            , IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 获取用户 Id
        /// </summary>
        public int UserId { get => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UserId").Value); }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public User User { get => _userRepository.Find(UserId); }

        /// <summary>
        /// 检查用户是否有效
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        public async Task<User> CheckUserAsync(int userId, bool tracking = true)
        {
            // 查询用户是否存在
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId, tracking);
            return user ?? throw Oops.Oh(SystemErrorCodes.u1002);
        }

        /// <summary>
        /// 查询用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Security>> GetSecuritiesAsync(int userId)
        {
            return await _memoryCache.GetOrCreateAsync($"{userId}_securities", async (_) =>
            {
                return await _userRepository.Include(u => u.Roles, false)
                                                .ThenInclude(u => u.Securities)
                                            .Where(u => u.Id == userId)
                                            .SelectMany(u => u.Roles
                                                .SelectMany(u => u.Securities).Distinct())
                                            .ToListAsync();
            });
        }
    }
}