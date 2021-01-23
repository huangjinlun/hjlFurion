using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Core
{
    public class SchoolClass: Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SchoolClass()
        {
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
        }

        public string ClassName { get; set; }


        /// <summary>
        /// 多对多 班级对应老师
        /// </summary>
        public ICollection<Teacher> Teachers { get; set; }

        /// <summary>
        /// 多对多中间表 老师可以教多个班级
        /// </summary>
        public ICollection<ClassTeacher> ClassTeachers { get; set; }

    }
}
