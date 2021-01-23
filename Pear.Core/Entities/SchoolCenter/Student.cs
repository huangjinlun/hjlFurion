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
    public class Student : Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Student()
        {
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 一个班级可以有多个学生
        /// </summary>
        public SchoolClass SchoolClass { get; set; }
    }
}
