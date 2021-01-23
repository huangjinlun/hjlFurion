using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 修改密码参数
    /// </summary>
    public class ChangePasswordInput : IValidatableObject
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required(ErrorMessage = "旧密码不能为空")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空"), StringLength(32, MinimumLength = 5, ErrorMessage = "新密码需在 5 到 32 个字符之间")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空"), Compare(nameof(Password), ErrorMessage = "两次密码输入不一致")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 验证旧密码和新密码
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OldPassword.Equals(Password))
            {
                yield return new ValidationResult(
                    "旧密码不能和新密码一致"
                    , new[] { nameof(Password) }
                );
            }
        }
    }
}