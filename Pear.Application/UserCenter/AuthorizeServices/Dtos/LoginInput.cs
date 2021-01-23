using Furion.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 登录模型
    /// </summary>
    [SkipScan]
    public class LoginInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空"), MinLength(3, ErrorMessage = "用户名不能少于 3 位字符")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空"), MinLength(5, ErrorMessage = "密码不能少于 5 位字符")]
        public string Password { get; set; }
    }
}