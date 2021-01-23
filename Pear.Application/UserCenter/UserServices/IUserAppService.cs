using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserAppService
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        UserProfile Profile();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<UserProfile> ProfileAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id")] int userId);

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedList<UserProfile>> GetListAsync([FromQuery, Required] GetUserListInput input);

        /// <summary>
        /// 修改当前用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyAsync([Required] ModifyUserInput input);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task DeleteAsync([Required, MinLength(1), MaxLength(20)] int[] userIds);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserProfile> AddAsync([Required] AddUserInput input);

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId, [Required] ModifyUserInput input);

        /// <summary>
        /// 修改当前用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangePassword([Required] ChangePasswordInput input);

        /// <summary>
        /// 查看当前用户拥有的角色
        /// </summary>
        /// <returns></returns>
        Task<List<RoleProfile>> GetRolesAsync();

        /// <summary>
        /// 查看用户拥有的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<RoleProfile>> GetRolesAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId);

        /// <summary>
        /// 查看当前用户拥有的权限
        /// </summary>
        /// <returns></returns>
        Task<List<SecurityProfile>> GetSecuritiesAsync();

        /// <summary>
        /// 查看用户拥有的权限
        /// </summary>
        /// <returns></returns>
        Task<List<SecurityProfile>> GetSecuritiesAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId);
    }
}