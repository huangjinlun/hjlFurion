namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 数据字典分类信息
    /// </summary>
    public class SystemDataCategoryProfile
    {
        /// <summary>
        /// 分类 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 序号/排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 上级分类 Id
        /// </summary>
        public int? HigherId { get; set; }
    }
}