using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pear.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 角色服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroupConsts.USER_CENTER)]
    public class RoleAppService : IRoleAppService, IDynamicApiController, ITransient
    {
        /// <summary>
        /// 角色仓储
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        /// <summary>
        /// 用户管理
        /// </summary>
        private readonly IUserManager _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleRepository"></param>
        /// <param name="userManager"></param>
        public RoleAppService(IRepository<Role> roleRepository
            , IUserManager userManager)
        {
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.ROLE_SERVICE_LIST), HttpGet, ApiDescriptionSettings(Name = "list")]
        public async Task<PagedList<RoleProfile>> GetListAsync([FromQuery, Required] GetRoleListInput input)
        {
            var hasKeyword = !string.IsNullOrEmpty(input.Keyword?.Trim());

            var roles = await _roleRepository.Where(
                                                (hasKeyword, u => EF.Functions.Like(u.Name, $"%{input.Keyword.Trim()}%")),
                                                (hasKeyword, u => EF.Functions.Like(u.Remark, $"%{input.Keyword.Trim()}%"))
                                              )
                                             .ToPagedListAsync(input.PageIndex, input.PageSize);

            return roles.Adapt<PagedList<RoleProfile>>();
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.ROLE_SERVICE_ADD), ApiDescriptionSettings(KeepVerb = true)]
        public async Task<RoleProfile> AddAsync([Required] EditRoleInput input)
        {
            // 判断角色是否存在
            var isExist = await _roleRepository.AnyAsync(u => u.Name.Trim().Equals(input.Name.Trim()));
            if (isExist) throw Oops.Oh(SystemErrorCodes.u1006);

            var addRole = input.Adapt<Role>();

            var entryEntity = await _roleRepository.InsertNowAsync(addRole);
            return entryEntity.Entity.Adapt<RoleProfile>();
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.ROLE_SERVICE_MODIFY), ApiDescriptionSettings(KeepVerb = true)]
        public async Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的角色 Id"), ApiSeat(ApiSeats.ActionStart)] int roleId, [Required] EditRoleInput input)
        {
            // 查询角色是否存在
            var role = await _roleRepository.FirstOrDefaultAsync(u => u.Id == roleId, false);
            _ = role ?? throw Oops.Oh(SystemErrorCodes.u1002);

            var modifyRole = input.Adapt<Role>();

            // 配置主键和更新时间
            modifyRole.Id = roleId;
            modifyRole.UpdatedTime = DateTimeOffset.Now;

            await _roleRepository.UpdateExcludeAsync(modifyRole, new[] { nameof(Role.IsDeleted), nameof(Role.CreatedTime) }, ignoreNullValues: true);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.ROLE_SERVICE_DELETE)]
        public async Task DeleteAsync([Required, MinLength(1), MaxLength(20)] int[] roleIds)
        {
            // 判断角色Id中是否有重复项
            var isRepeat = roleIds.GroupBy(i => i).Any(g => g.Count() > 1);
            if (isRepeat) throw Oops.Oh(SystemErrorCodes.u1011);

            // 查询所有存在且不被引用的角色
            var roles = await _roleRepository.Include(u => u.Users)
                                             .Include(u => u.Securities)
                                             .Where(u => roleIds.Contains(u.Id) && !(u.Users.Any() || u.Securities.Any()))
                                             .ToListAsync();

            // 判断要操作的数据长度是是否等于传入数据的长度
            if (roleIds.Length != roles.Count) throw Oops.Oh(SystemErrorCodes.u1007);

            var nowTime = DateTimeOffset.Now;
            roles.ForEach(u =>
            {
                u.UpdatedTime = nowTime;
                u.IsDeleted = true;
            });
        }

        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        [SecurityDefine(SecurityConsts.ROLE_SERVICE_GIVE)]
        public async Task GiveAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id")] int userId, [Required, MinLength(1)] int[] roleIds)
        {
            // 查询用户是否存在
            var user = await _userManager.CheckUserAsync(userId, false);

            // 禁止为管理员分配角色
            if (user.Account == "admin") throw Oops.Oh(SystemErrorCodes.u1008);

            // 判断传入角色数据是否正确
            var count = await _roleRepository.CountAsync(u => roleIds.Contains(u.Id), false);
            if (roleIds.Contains(0) || count != roleIds.Length) throw Oops.Oh(SystemErrorCodes.u1009);

            // 删除已有的角色
            var userRoleRepository = _roleRepository.Change<UserRole>();
            await userRoleRepository.DeleteAsync(userRoleRepository.Where(u => u.UserId == userId, false).ToList());

            var list = new List<UserRole>();
            foreach (var roleid in roleIds)
            {
                list.Add(new UserRole { UserId = userId, RoleId = roleid });
            }

            await userRoleRepository.InsertAsync(list);
        }

        /// <summary>
        /// 获取角色拥有权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<SecurityProfile>> GetSecuritiesAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的角色 Id"), ApiSeat(ApiSeats.ActionStart)] int roleId)
        {
            // 查询角色是否存在
            var role = await _roleRepository.FirstOrDefaultAsync(u => u.Id == roleId, false);
            _ = role ?? throw Oops.Oh(SystemErrorCodes.u1002);

            var securities = await _roleRepository.Include(u => u.Securities, false)
                                                  .SelectMany(u => u.Securities)
                                                  .ToListAsync();

            return securities.Adapt<List<SecurityProfile>>();
        }
    }
}