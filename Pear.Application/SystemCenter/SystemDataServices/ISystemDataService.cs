using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 数据字典接口
    /// </summary>
    public interface ISystemDataService
    {
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        Task<List<SystemDataCategoryProfile>> GetCategoriesAsync();

        /// <summary>
        /// 获取分类数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<SystemDataProfile>> GetDataAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId);

        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<SystemDataCategoryProfile> GetCategoryAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId);

        /// <summary>
        /// 修改分类信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId, [Required] EditSystemDataCategoryInput input);

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SystemDataCategoryProfile> AddAsync([Required] EditSystemDataCategoryInput input);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task DeleteAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId);

        /// <summary>
        /// 新增字典数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SystemDataProfile> AddDictionaryDataAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典分类Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId, [Required] EditSystemDataInput input);

        /// <summary>
        /// 更新字典数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="dataId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateDictionaryDataAsync(
            [Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典分类Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId,
            [Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典数据Id")] int dataId,
            [Required] EditSystemDataInput input);

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="dataIds"></param>
        /// <returns></returns>
        Task DeleteDictionaryDataAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典分类Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId,
            [Required, MinLength(1), MaxLength(20)] int[] dataIds);
    }
}