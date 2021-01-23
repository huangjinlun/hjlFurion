using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Application.SchoolCenter
{
    public interface ISchoolClassAppService
    {
        /// <summary>
        /// 新增班级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SchoolClassProfile> AddClassAsync([Required] AddSchoolClassInput input);

        /// <summary>
        /// 编辑班级信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的班级 Id"), ApiSeat(ApiSeats.ActionStart)] int userId, [Required] ModifySchoolClassInput input);


        /// <summary>
        /// 获取所有班级列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedList<SchoolClassProfile>> GetListAsync([FromQuery, Required] GetSchoolClassListInput input);

        /// <summary>
        /// 删除班级
        /// </summary>
        /// <param name="calssIds"></param>
        /// <returns></returns>
        Task DeleteAsync([Required, MinLength(1), MaxLength(20)] int[] calssIds);
    }
}
