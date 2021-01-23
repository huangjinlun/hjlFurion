using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Core
{
    /// <summary>
    /// 班级、老师多对多关系中间实体
    /// </summary>
    public class ClassTeacher : IEntity
    {
        public int SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
