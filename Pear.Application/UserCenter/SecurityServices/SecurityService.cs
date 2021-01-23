using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pear.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 权限服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroupConsts.USER_CENTER)]
    public class SecurityService : ISecurityService, IDynamicApiController, ITransient
    {
        /// <summary>
        /// 权限仓储
        /// </summary>
        private readonly IRepository<Security> _securityRepository;

        /// <summary>
        /// 角色仓储
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        /// <summary>
        /// 内存缓存
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 用户管理
        /// </summary>
        private readonly IUserManager _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="securityRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="memoryCache"></param>
        /// <param name="userManager"></param>
        public SecurityService(IRepository<Security> securityRepository
            , IRepository<Role> roleRepository
            , IMemoryCache memoryCache
            , IUserManager userManager)
        {
            _securityRepository = securityRepository;
            _roleRepository = roleRepository;
            _memoryCache = memoryCache;
            _userManager = userManager;
        }

        /// <summary>
        /// 刷新用户权限缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.SECURITY_SERVICE_REFRESH)]
        public async Task Refresh([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId)
        {
            // 查询用户是否存在
            _ = await _userManager.CheckUserAsync(userId, false);
            _memoryCache.Remove($"{userId}_securities");
        }

        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.SECURITY_SERVICE_LIST), HttpGet, ApiDescriptionSettings(Name = "list")]
        public async Task<PagedList<SecurityProfile>> GetListAsync([FromQuery, Required] GetSecurityListInput input)
        {
            var hasKeyword = !string.IsNullOrEmpty(input.Keyword?.Trim());

            var securities = await _securityRepository.Where(
                                                (hasKeyword, u => EF.Functions.Like(u.UniqueName, $"%{input.Keyword.Trim()}%")),
                                                (hasKeyword, u => EF.Functions.Like(u.Remark, $"%{input.Keyword.Trim()}%"))
                                              )
                                             .ToPagedListAsync(input.PageIndex, input.PageSize);

            return securities.Adapt<PagedList<SecurityProfile>>();
        }

        /// <summary>
        /// 获取所有权限列表（不分页）
        /// </summary>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.SECURITY_SERVICE_ALL), HttpGet, ApiDescriptionSettings(Name = "all")]
        public async Task<List<SecurityProfile>> GetAllAsync()
        {
            var securities = await _securityRepository.AsAsyncEnumerable(false);
            return securities.Adapt<List<SecurityProfile>>();
        }

        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="securityIds"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.SECURITY_SERVICE_GIVE)]
        public async Task GiveAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的角色 Id")] int roleId, [Required, MinLength(1)] int[] securityIds)
        {
            // 禁止为超级管理员分配角色权限
            if (roleId == 1) throw Oops.Oh(SystemErrorCodes.u1010);

            // 查询角色是否存在
            var role = await _roleRepository.FirstOrDefaultAsync(u => u.Id == roleId, false);
            _ = role ?? throw Oops.Oh(SystemErrorCodes.u1002);

            // 判断传入权限数据是否正确
            var count = await _securityRepository.CountAsync(u => securityIds.Contains(u.Id), false);
            if (securityIds.Contains(0) || count != securityIds.Length) throw Oops.Oh(SystemErrorCodes.u1009);

            // 删除已有的权限
            var roleSecurityRepository = _roleRepository.Change<RoleSecurity>();
            await roleSecurityRepository.DeleteAsync(roleSecurityRepository.Where(u => u.RoleId == roleId, false).ToList());

            var list = new List<RoleSecurity>();
            foreach (var securityId in securityIds)
            {
                list.Add(new RoleSecurity { RoleId = roleId, SecurityId = securityId });
            }

            await roleSecurityRepository.InsertAsync(list);
        }
    }
}