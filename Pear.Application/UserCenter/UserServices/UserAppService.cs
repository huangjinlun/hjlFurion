using Furion.DatabaseAccessor;
using Furion.DataEncryption;
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
    /// 用户服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroupConsts.USER_CENTER)]
    public class UserAppService : IUserAppService, IDynamicApiController, ITransient
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// 用户管理类
        /// </summary>
        private readonly IUserManager _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="userManager"></param>
        public UserAppService(IRepository<User> userRepository
            , IUserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_PROFILE_SELF), HttpGet]
        public UserProfile Profile()
        {
            return _userManager.User.Adapt<UserProfile>();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_PROFILE), HttpGet]
        public async Task<UserProfile> ProfileAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id")] int userId)
        {
            // 查询用户是否存在
            var user = await _userManager.CheckUserAsync(userId, false);
            return user.Adapt<UserProfile>();
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_LIST), HttpGet, ApiDescriptionSettings(Name = "list")]
        public async Task<PagedList<UserProfile>> GetListAsync([FromQuery, Required] GetUserListInput input)
        {
            var hasKeyword = !string.IsNullOrEmpty(input.Keyword?.Trim());

            var users = await _userRepository.Where(
                                                (hasKeyword, u => EF.Functions.Like(u.Account, $"%{input.Keyword.Trim()}%")),
                                                (hasKeyword, u => EF.Functions.Like(u.Nickname, $"%{input.Keyword.Trim()}%"))
                                              )
                                             .ToPagedListAsync(input.PageIndex, input.PageSize);

            return users.Adapt<PagedList<UserProfile>>();
        }

        /// <summary>
        /// 修改当前用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_MODIFY_SELF), HttpPut]
        public async Task ModifyAsync([Required] ModifyUserInput input)
        {
            var user = input.Adapt<User>();

            // 配置主键和更新时间
            user.Id = _userManager.UserId;
            user.UpdatedTime = DateTimeOffset.Now;

            await _userRepository.UpdateExcludeAsync(user, new[] { nameof(User.IsDeleted), nameof(user.CreatedTime) }, ignoreNullValues: true);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_DELETE)]
        public async Task DeleteAsync([Required, MinLength(1), MaxLength(20)] int[] userIds)
        {
            // 判断用户Id中是否有重复项
            var isRepeat = userIds.GroupBy(i => i).Any(g => g.Count() > 1);
            if (isRepeat) throw Oops.Oh(SystemErrorCodes.u1011);

            // 禁止删除自己
            if (userIds.Contains(_userManager.UserId)) throw Oops.Oh(SystemErrorCodes.u1001);

            // 查询所有存在且不被引用的用户
            var users = await _userRepository.Include(u => u.Roles)
                                             .Where(u => userIds.Contains(u.Id) && !u.Roles.Any())
                                             .ToListAsync();

            // 判断要操作的数据长度是是否等于传入数据的长度
            if (userIds.Length != users.Count) throw Oops.Oh(SystemErrorCodes.u1007);

            var nowTime = DateTimeOffset.Now;
            users.ForEach(u =>
            {
                u.UpdatedTime = nowTime;
                u.IsDeleted = true;
            });
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_ADD), ApiDescriptionSettings(KeepVerb = true)]
        public async Task<UserProfile> AddAsync([Required] AddUserInput input)
        {
            // 判断账号是否存在
            var isExist = await _userRepository.AnyAsync(u => u.Account.ToLower() == input.Account.Trim().ToLower());
            if (isExist) throw Oops.Oh(SystemErrorCodes.u1003);

            var addUser = input.Adapt<User>();
            addUser.Password = MD5Encryption.Encrypt(input.Password.Trim());

            var entryEntity = await _userRepository.InsertNowAsync(addUser);
            return entryEntity.Entity.Adapt<UserProfile>();
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_MODIFY), HttpPut]
        public async Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId, [Required] ModifyUserInput input)
        {
            // 查询用户是否存在
            _ = await _userManager.CheckUserAsync(userId, false);

            var user = input.Adapt<User>();

            // 配置主键和更新时间
            user.Id = userId;
            user.UpdatedTime = DateTimeOffset.Now;

            await _userRepository.UpdateExcludeAsync(user, new[] { nameof(User.IsDeleted), nameof(User.CreatedTime) }, ignoreNullValues: true);
        }

        /// <summary>
        /// 修改当前用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_CHANGE_PASSWORD)]
        public Task ChangePassword([Required] ChangePasswordInput input)
        {
            var user = _userManager.User;

            var oldEncryptPassword = MD5Encryption.Encrypt(input.OldPassword.Trim());
            if (!user.Password.Equals(oldEncryptPassword)) throw Oops.Oh(SystemErrorCodes.u1004);

            // 禁止被修改 admin 密码
            if (user.Account.Equals("admin")) throw Oops.Oh(SystemErrorCodes.u1005);

            user.Password = MD5Encryption.Encrypt(input.Password.Trim());
            user.UpdatedTime = DateTimeOffset.Now;

            return Task.CompletedTask;
        }

        /// <summary>
        /// 查看当前用户拥有的角色
        /// </summary>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_ROLES_SELF)]
        public async Task<List<RoleProfile>> GetRolesAsync()
        {
            var userId = _userManager.UserId;

            var roles = await _userRepository.DetachedEntities
                                            .Include(u => u.Roles)
                                            .Where(u => u.Id == userId)
                                            .SelectMany(u => u.Roles)
                                            .ProjectToType<RoleProfile>()
                                            .ToListAsync();

            return roles;
        }

        /// <summary>
        /// 查看用户拥有的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_ROLES)]
        public async Task<List<RoleProfile>> GetRolesAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId)
        {
            // 查询用户是否存在
            var user = await _userManager.CheckUserAsync(userId, false);

            var roles = await _userRepository.DetachedEntities
                                            .Include(u => u.Roles)
                                            .Where(u => u.Id == userId)
                                            .SelectMany(u => u.Roles)
                                            .ProjectToType<RoleProfile>()
                                            .ToListAsync();

            return roles;
        }

        /// <summary>
        /// 查看当前用户拥有的权限
        /// </summary>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_SECURITIES_SELF)]
        public async Task<List<SecurityProfile>> GetSecuritiesAsync()
        {
            var securities = await _userManager.GetSecuritiesAsync(_userManager.UserId);
            return securities.Adapt<List<SecurityProfile>>();
        }

        /// <summary>
        /// 查看用户拥有的权限
        /// </summary>
        /// <returns></returns>
        [SecurityDefine(SecurityConsts.USER_SERVICE_SECURITIES)]
        public async Task<List<SecurityProfile>> GetSecuritiesAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId)
        {
            // 查询用户是否存在
            _ = await _userManager.CheckUserAsync(userId, false);

            var securities = await _userManager.GetSecuritiesAsync(userId);
            return securities.Adapt<List<SecurityProfile>>();
        }
    }
}