namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 数据字典信息
    /// </summary>
    public class SystemDataProfile
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
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
        /// 所属分类Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 所属分类名
        /// </summary>
        public string CategoryName { get; set; }
    }
}