using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 角色服务接口
    /// </summary>
    public interface IRoleAppService
    {
        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedList<RoleProfile>> GetListAsync([FromQuery, Required] GetRoleListInput input);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RoleProfile> AddAsync([Required] EditRoleInput input);

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的角色 Id"), ApiSeat(ApiSeats.ActionStart)] int roleId, [Required] EditRoleInput input);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task DeleteAsync([Required, MinLength(1), MaxLength(20)] int[] roleIds);

        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        Task GiveAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id")] int userId, [Required, MinLength(1)] int[] roleIds);

        /// <summary>
        /// 获取角色拥有权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<SecurityProfile>> GetSecuritiesAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的角色 Id"), ApiSeat(ApiSeats.ActionStart)] int roleId);
    }
}