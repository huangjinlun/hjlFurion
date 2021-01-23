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

namespace Pear.Application.SchoolCenter.SchoolClassService
{
    [ApiDescriptionSettings(ApiGroupConsts.SCHOOL_CENTER, Name = "scho")]
    public class SchoolClassAppService : ISchoolClassAppService, IDynamicApiController, ITransient
    {

        /// <summary>
        /// 班级仓储
        /// </summary>
        private readonly IRepository<SchoolClass> _classRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="classRepository"></param>
        public SchoolClassAppService(IRepository<SchoolClass> classRepository)
        {
            _classRepository = classRepository;
        }

        /// <summary>
        /// 新增班级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SchoolClassProfile> AddClassAsync([Required] AddSchoolClassInput input)
        {
            // 判断账号是否存在
            var isExist = await _classRepository.AnyAsync(u => u.ClassName.ToLower() == input.ClassName.Trim().ToLower());
            if (isExist) throw Oops.Oh(SystemErrorCodes.u1003);

            var addClass = input.Adapt<SchoolClass>();

            var entryEntity = await _classRepository.InsertNowAsync(addClass);
            return entryEntity.Entity.Adapt<SchoolClassProfile>();
        }

        /// <summary>
        /// 获取所有班级列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedList<SchoolClassProfile>> GetListAsync([FromQuery, Required] GetSchoolClassListInput input)
        {
            var teachers = _classRepository.Include(u => u.Teachers);
            var classTeachers = _classRepository.Include(u => u.ClassTeachers);

            var hasKeyword = !string.IsNullOrEmpty(input.Keyword?.Trim());

            var users = await _classRepository.Where(
                                                (hasKeyword, u => EF.Functions.Like(u.ClassName, $"%{input.Keyword.Trim()}%"))
                                              )
                                             .ToPagedListAsync(input.PageIndex, input.PageSize);

            return users.Adapt<PagedList<SchoolClassProfile>>();
        }

        /// <summary>
        /// 编辑班级
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ModifyAsync([ApiSeat(ApiSeats.ActionStart), Range(1, int.MaxValue, ErrorMessage = "请输入有效的班级 Id"), Required] int classId, [Required] ModifySchoolClassInput input)
        {
            var classInfo = input.Adapt<SchoolClass>();

            // 配置主键和更新时间
            classInfo.Id = classId;
            classInfo.UpdatedTime = DateTimeOffset.Now;

            await _classRepository.UpdateExcludeAsync(classInfo, new[] { nameof(SchoolClass.IsDeleted), nameof(SchoolClass.CreatedTime) }, ignoreNullValues: true);
        }

        /// <summary>
        /// 删除班级
        /// </summary>
        /// <param name="classIds"></param>
        /// <returns></returns>
        public async Task DeleteAsync([Required, MinLength(1), MaxLength(20)] int[] classIds)
        {
            // 判断用户Id中是否有重复项
            var isRepeat = classIds.GroupBy(i => i).Any(g => g.Count() > 1);
            if (isRepeat) throw Oops.Oh(SystemErrorCodes.u1011);

            // 查询所有存在且不被引用的用户
            var classes = await _classRepository
                                             .Where(u => classIds.Contains(u.Id))
                                             .ToListAsync();

            // 判断要操作的数据长度是是否等于传入数据的长度
            if (classIds.Length != classes.Count) throw Oops.Oh(SystemErrorCodes.u1007);

            var nowTime = DateTimeOffset.Now;
            classes.ForEach(u =>
            {
                u.UpdatedTime = nowTime;
                u.IsDeleted = true;
            });
        }
    }
}
