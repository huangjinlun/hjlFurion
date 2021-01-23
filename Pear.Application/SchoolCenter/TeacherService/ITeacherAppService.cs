using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Application.SchoolCenter
{
    public interface ITeacherAppService
    {
        /// <summary>
        /// 获取所有老师列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedList<TeacherProfile>> GetListAsync([FromQuery, Required] GetTeacherListInput input);

        /// <summary>
        /// 新增老师
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TeacherProfile> AddAsync([Required] AddTeacherInput input);

        /// <summary>
        /// 编辑老师信息
        /// </summary>
        /// <param name="goodsInfoId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的老师 Id"), ApiSeat(ApiSeats.ActionStart)] int goodsInfoId, [Required] ModifyTeacherInput input);

        /// <summary>
        /// 为老师分配受教班级
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="teacherds"></param>
        Task GiveAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的班级、老师 Id")] int classId, [Required, MinLength(1)] int[] teacherds);
    }
}
