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
    public class Teacher : Entity, IEntityTypeBuilder<Teacher>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Teacher()
        {
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
        }

        public string TeacherName { get; set; }

        /// <summary>
        /// 多对多 班级对老师
        /// </summary>
        public ICollection<SchoolClass> SchoolClasss { get; set; }

        /// <summary>
        /// 多对多中间表
        /// </summary>
        public ICollection<ClassTeacher> ClassTeachers { get; set; }

        public void Configure(EntityTypeBuilder<Teacher> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(p => p.SchoolClasss)
                 .WithMany(p => p.Teachers)
                 .UsingEntity<ClassTeacher>(
                   u => u.HasOne(c => c.SchoolClass).WithMany(c => c.ClassTeachers).HasForeignKey(c => c.SchoolClassId)
                 , u => u.HasOne(c => c.Teacher).WithMany(c => c.ClassTeachers).HasForeignKey(c => c.TeacherId)
                 , u =>
                 {
                     u.HasKey(c => new { c.SchoolClassId, c.TeacherId });
                 });
        }
    }
}
