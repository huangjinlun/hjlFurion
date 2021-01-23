using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Pear.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Furion.FriendlyException;

namespace Pear.Application.SchoolCenter
{
    [ApiDescriptionSettings(ApiGroupConsts.SCHOOL_CENTER)]
    public class StudentAppService : IStudentAppService, IDynamicApiController, ITransient
    {
        private readonly IRepository<Pear.Core.Student> _studnetRepository;
        private readonly IRepository<SchoolClass> _schoolClassRepository;
        private readonly IRepository<Teacher> _teacherRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="studnetRepository"></param>
        /// <param name="schoolClassRepository"></param>
        ///  <param name="teacherRepository"></param>
        public StudentAppService(IRepository<Student> studnetRepository, IRepository<SchoolClass> schoolClassRepository, IRepository<Teacher> teacherRepository)
        {
            _studnetRepository = studnetRepository;
            _schoolClassRepository = schoolClassRepository;
            _teacherRepository = teacherRepository;
        }

        /// <summary>
        /// （复杂查询）查询分配班级的老师受教的学生分页列表
        /// </summary>
        /// <param name="input"></param>
        public async Task<PagedList<StudentProfile>> GetListAsync([FromQuery, Required] GetStudentListInput input)
        {
            var classTeacherRepository = _teacherRepository.Change<ClassTeacher>();

            var hasKeyword = !string.IsNullOrEmpty(input.Keyword?.Trim());
            var query = from p in _studnetRepository.AsQueryable()
                        join d in _schoolClassRepository.AsQueryable() on p.SchoolClass.Id equals d.Id
                        join c in classTeacherRepository.AsQueryable() on p.SchoolClass.Id equals c.SchoolClassId
                        join e in _teacherRepository.AsQueryable() on c.TeacherId equals e.Id
                        select new StudentProfile
                        {
                            SchoolClassName = d.ClassName,
                            TeacherName = e.TeacherName,
                            Name = p.Name,
                            Age = p.Age,
                            Id = p.Id,
                            Birthday=p.Birthday,
                            SchoolClassId=d.Id
                        };
            var tempStudents = await query.Where(
                                                (hasKeyword, g => EF.Functions.Like(g.Name, $"%{input.Keyword.Trim()}%"))
                                              ).ToPagedListAsync(input.PageIndex, input.PageSize);

            return tempStudents.Adapt<PagedList<StudentProfile>>();
        }

        public async Task<StudentProfile> AddAsync([Required] AddStudentInput input)
        {
            // 判断账号是否存在
            var isExist = await _studnetRepository.AnyAsync(u => u.Name.ToLower() == input.Name.Trim().ToLower());
            if (isExist) throw Oops.Oh(SystemErrorCodes.u1003);

            var addStudent = input.Adapt<Student>();
            addStudent.SchoolClass = _schoolClassRepository.Entities.Find(input.SchoolClassId);
            var entryEntity = await _studnetRepository.InsertNowAsync(addStudent);
            return entryEntity.Entity.Adapt<StudentProfile>();
        }

        public async Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的学生 Id"), ApiSeat(ApiSeats.ActionStart)] int studentId, [Required] ModifyStudentInput input)
        {
            var student = input.Adapt<Student>();

            // 配置主键和更新时间
            student.Id = studentId;
            student.UpdatedTime = DateTimeOffset.Now;

            await _studnetRepository.UpdateExcludeAsync(student, new[] { nameof(student.IsDeleted), nameof(student.CreatedTime) }, ignoreNullValues: true);
        }
    }
}
