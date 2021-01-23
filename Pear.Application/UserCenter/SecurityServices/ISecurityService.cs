using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 权限服务接口
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// 刷新用户权限缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task Refresh([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的用户 Id"), ApiSeat(ApiSeats.ActionStart)] int userId);

        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedList<SecurityProfile>> GetListAsync([FromQuery, Required] GetSecurityListInput input);

        /// <summary>
        /// 获取所有权限列表（不分页）
        /// </summary>
        /// <returns></returns>
        Task<List<SecurityProfile>> GetAllAsync();

        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="securityIds"></param>
        /// <returns></returns>
        Task GiveAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的角色 Id")] int roleId, [Required, MinLength(1)] int[] securityIds);
    }
}