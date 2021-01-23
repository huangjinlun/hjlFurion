using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 授权服务接口
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="input"></param>
        /// <remarks>
        /// 用户名/密码：admin/admin
        /// </remarks>
        /// <returns></returns>
        Task<LoginOutput> LoginAsync([FromServices] IHttpContextAccessor httpContextAccessor, [Required] LoginInput input);
    }
}