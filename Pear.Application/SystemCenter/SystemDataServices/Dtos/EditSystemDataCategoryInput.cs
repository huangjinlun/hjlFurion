using System.ComponentModel.DataAnnotations;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 数据字典分类信息
    /// </summary>
    public class EditSystemDataCategoryInput
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空"), StringLength(32, MinimumLength = 2, ErrorMessage = "名称需在 2 到 32 个字符之间")]
        public string Name { get; set; }

        /// <summary>
        /// 分类描述
        /// </summary>
        [StringLength(200, ErrorMessage = "描述最大只能输入 100 个字符")]
        public string Remark { get; set; }

        /// <summary>
        /// 序号/排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 上级分类 Id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "上级分类Id编号从1开始或为空")]
        public int? HigherId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}