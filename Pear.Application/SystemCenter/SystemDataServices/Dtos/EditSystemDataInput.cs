using System.ComponentModel.DataAnnotations;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 数据字典信息输入参数
    /// </summary>
    public class EditSystemDataInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空"), StringLength(32, MinimumLength = 2, ErrorMessage = "名称需在2到32个字符之间")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(100, ErrorMessage = "描述最大只能输入100个字符")]
        public string Remark { get; set; }

        /// <summary>
        /// 序号/排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}