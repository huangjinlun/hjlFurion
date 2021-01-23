using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pear.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pear.Application.SchoolCenter
{
    [ApiDescriptionSettings(ApiGroupConsts.SCHOOL_CENTER)]
    public class TeacherAppService : ITeacherAppService, IDynamicApiController, ITransient
    {
        private readonly IRepository<Student> _studnetRepository;
        private readonly IRepository<Teacher> _teacherRepository;
        /// <summary>
        /// 老师管理类
        /// </summary>
        private readonly IUserManager _userManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="studnetRepository"></param>
        /// <param name="teacherRepository"></param>
        /// <param name="userManager"></param>
        public TeacherAppService(IRepository<Student> studnetRepository, IRepository<Teacher> teacherRepository, IUserManager userManager)
        {
            _studnetRepository = studnetRepository;
            _teacherRepository = teacherRepository;
            _userManager = userManager;
        }

        public async Task<TeacherProfile> AddAsync([Required] AddTeacherInput input)
        {
            // 判断账号是否存在
            var isExist = await _teacherRepository.AnyAsync(u => u.TeacherName.ToLower() == input.TeacherName.Trim().ToLower());
            if (isExist) throw Oops.Oh(SystemErrorCodes.u1003);

            var addTeacher = input.Adapt<Teacher>();
            var entryEntity = await _teacherRepository.InsertNowAsync(addTeacher);
            return entryEntity.Entity.Adapt<TeacherProfile>();
        }

        public async Task<PagedList<TeacherProfile>> GetListAsync([FromQuery, Required] GetTeacherListInput input)
        {
            var hasKeyword = !string.IsNullOrEmpty(input.Keyword?.Trim());

            var student = await _teacherRepository.Where(
                                                (hasKeyword, g => EF.Functions.Like(g.TeacherName, $"%{input.Keyword.Trim()}%"))
                                              )
                                             .ToPagedListAsync(input.PageIndex, input.PageSize);

            return student.Adapt<PagedList<TeacherProfile>>();
        }

        public async Task ModifyAsync([ApiSeat(ApiSeats.ActionStart), Range(1, int.MaxValue, ErrorMessage = "请输入有效的老师 Id"), Required] int Id, [Required] ModifyTeacherInput input)
        {
            var teacher = input.Adapt<Pear.Core.Teacher>();

            // 配置主键和更新时间
            teacher.Id = Id;
            teacher.UpdatedTime = DateTimeOffset.Now;

            await _teacherRepository.UpdateExcludeAsync(teacher, new[] { nameof(teacher.IsDeleted), nameof(teacher.CreatedTime) }, ignoreNullValues: true);
        }


        /// <summary>
        /// 为老师分配受教班级
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="teacherds"></param>
        public async Task GiveAsync([Range(1, int.MaxValue, ErrorMessage = "请输入有效的班级、老师 Id"), Required] int classId, [MinLength(1), Required] int[] teacherds)
        {
            // 判断传入老师数据是否正确
            var count = await _teacherRepository.CountAsync(u => teacherds.Contains(u.Id), false);
            if (teacherds.Contains(0) || count != teacherds.Length) throw Oops.Oh(SystemErrorCodes.u1009);

            // 删除已有的老师
            var classTeacherRepository = _teacherRepository.Change<ClassTeacher>();
            await classTeacherRepository.DeleteAsync(classTeacherRepository.Where(u => u.SchoolClassId == classId, false).ToList());

            var list = new List<ClassTeacher>();
            foreach (var techerId in teacherds)
            {
                list.Add(new ClassTeacher { TeacherId = techerId, SchoolClassId = classId });
            }

            await classTeacherRepository.InsertAsync(list);
        }
    }
}
