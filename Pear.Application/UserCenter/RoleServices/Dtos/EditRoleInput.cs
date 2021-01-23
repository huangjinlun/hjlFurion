using System.ComponentModel.DataAnnotations;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 新增角色输入参数
    /// </summary>
    public class EditRoleInput
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空"), StringLength(32, MinimumLength = 2, ErrorMessage = "名称需在 2 到 32 个字符之间")]
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [StringLength(200, ErrorMessage = "描述最大只能输入 100 个字符")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}