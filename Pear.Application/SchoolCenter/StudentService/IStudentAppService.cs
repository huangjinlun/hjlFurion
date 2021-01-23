using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Application.SchoolCenter
{
    public interface IStudentAppService
    {
        /// <summary>
        /// 获取所有学生列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedList<StudentProfile>> GetListAsync([FromQuery, Required] GetStudentListInput input);

        /// <summary>
        /// 新增学生
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StudentProfile> AddAsync([Required] AddStudentInput input);

        /// <summary>
        /// 编辑学生信息
        /// </summary>
        /// <param name="goodsInfoId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的学生 Id"), ApiSeat(ApiSeats.ActionStart)] int goodsInfoId, [Required] ModifyStudentInput input);
    }
}
