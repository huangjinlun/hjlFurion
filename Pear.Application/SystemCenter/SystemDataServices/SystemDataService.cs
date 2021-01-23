using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pear.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 数据字典服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroupConsts.SYSTEM_CENTER, Name = "sysdata")]
    public class SystemDataService : ISystemDataService, IDynamicApiController, ITransient
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 分类仓储
        /// </summary>
        private readonly IRepository<SystemDataCategory> _categoryRepository;

        /// <summary>
        /// 数据仓储
        /// </summary>
        private readonly IRepository<SystemData> _dataRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="categoryRepository"></param>
        /// <param name="dataRepository"></param>
        /// <param name="memoryCache"></param>
        public SystemDataService(IRepository<SystemDataCategory> categoryRepository
            , IRepository<SystemData> dataRepository
            , IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _dataRepository = dataRepository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemDataCategoryProfile>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.AsAsyncEnumerable();
            return categories.Adapt<List<SystemDataCategoryProfile>>();
        }

        /// <summary>
        /// 获取分类数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<List<SystemDataProfile>> GetDataAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(u => u.Id == categoryId, false);
            _ = category ?? throw Oops.Oh(SystemErrorCodes.u1002);

            return await _dataRepository.DetachedEntities
                                    .Include(u => u.Category)
                                    .Where(u => u.CategoryId == categoryId)
                                    .Select(u => new SystemDataProfile
                                    {
                                        CategoryId = u.CategoryId,
                                        CategoryName = u.Category.Name,
                                        Name = u.Name,
                                        Remark = u.Remark,
                                        Id = u.Id,
                                        Sequence = u.Sequence,
                                        Enabled = u.Enabled
                                    })
                                    .ToListAsync();
        }

        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<SystemDataCategoryProfile> GetCategoryAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(u => u.Id == categoryId, false);
            _ = category ?? throw Oops.Oh(SystemErrorCodes.u1002);

            return category.Adapt<SystemDataCategoryProfile>();
        }

        /// <summary>
        /// 修改分类信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ModifyAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId, [Required] EditSystemDataCategoryInput input)
        {
            // 查询分类是否存在
            var isExist = await _categoryRepository.AnyAsync(u => u.Id == categoryId, false);
            if (!isExist) throw Oops.Oh(SystemErrorCodes.u1002);

            var modifyCategory = input.Adapt<SystemDataCategory>();

            // 配置主键和更新时间
            modifyCategory.Id = categoryId;
            modifyCategory.UpdatedTime = DateTimeOffset.Now;

            await _categoryRepository.UpdateExcludeAsync(modifyCategory, new[] { nameof(Role.IsDeleted), nameof(Role.CreatedTime) }, ignoreNullValues: true);
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ApiDescriptionSettings(KeepVerb = true)]
        public async Task<SystemDataCategoryProfile> AddAsync([Required] EditSystemDataCategoryInput input)
        {
            // 判断分类名是否存在
            var isExist = await _categoryRepository.AnyAsync(u => u.Name.Trim().Equals(input.Name.Trim()));
            if (isExist) throw Oops.Oh(SystemErrorCodes.u1006);

            var addCategory = input.Adapt<SystemDataCategory>();

            var entryEntity = await _categoryRepository.InsertNowAsync(addCategory);
            return entryEntity.Entity.Adapt<SystemDataCategoryProfile>();
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task DeleteAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的分类 Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId)
        {
            // 查询分类是否存在
            var deleteCategory = await _categoryRepository.FindAsync(categoryId);
            _ = deleteCategory ?? throw Oops.Oh(SystemErrorCodes.u1002);

            // 查询分类是否被其他数据引用
            var isRef = await _categoryRepository.Include(u => u.Sublevels, false)
                                                 .Include(u => u.Data)
                                                 .AnyAsync(u => u.Id == categoryId && (u.Sublevels.Any() || u.Data.Any()));

            if (isRef) throw Oops.Oh(SystemErrorCodes.u1007);

            // 软/假删除
            deleteCategory.UpdatedTime = DateTimeOffset.Now;
            deleteCategory.IsDeleted = true;
        }

        /// <summary>
        /// 新增字典数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SystemDataProfile> AddDictionaryDataAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典分类Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId, [Required] EditSystemDataInput input)
        {
            // 查询字典分类是否存在
            var isExist = await _categoryRepository.AnyAsync(u => u.Id == categoryId, false);
            if (!isExist) throw Oops.Oh(SystemErrorCodes.u1002);

            var dictionaryData = input.Adapt<SystemData>();
            // 配置分类主键
            dictionaryData.CategoryId = categoryId;

            var newObj = await _dataRepository.InsertNowAsync(dictionaryData);
            return newObj.Entity.Adapt<SystemDataProfile>();
        }

        /// <summary>
        /// 更新字典数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="dataId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateDictionaryDataAsync(
            [Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典分类Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId,
            [Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典数据Id")] int dataId,
            [Required] EditSystemDataInput input)
        {
            // 查询字典分类是否存在
            var isExist = await _categoryRepository.AnyAsync(u => u.Id == categoryId, false);
            if (!isExist) throw Oops.Oh(SystemErrorCodes.u1002);

            // 查询字典数据是否存在
            isExist = await _dataRepository.AnyAsync(u => u.Id == dataId && u.CategoryId == categoryId, false);
            if (!isExist) throw Oops.Oh(SystemErrorCodes.u1002);

            var dictionaryData = input.Adapt<SystemData>();
            // 配置主外键和更新时间
            dictionaryData.Id = dataId;
            dictionaryData.CategoryId = categoryId;
            dictionaryData.UpdatedTime = DateTimeOffset.Now;

            await _dataRepository.UpdateExcludeAsync(dictionaryData, new[] { nameof(SystemData.IsDeleted), nameof(SystemData.CreatedTime) }, ignoreNullValues: true);
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="dataIds"></param>
        /// <returns></returns>
        public async Task DeleteDictionaryDataAsync([Required, Range(1, int.MaxValue, ErrorMessage = "请输入有效的字典分类Id"), ApiSeat(ApiSeats.ActionStart)] int categoryId,
            [Required, MinLength(1), MaxLength(20)] int[] dataIds)
        {
            // 查询字典分类是否存在
            var isExist = await _categoryRepository.AnyAsync(u => u.Id == categoryId, false);
            if (!isExist) throw Oops.Oh(SystemErrorCodes.u1002);

            var dictionaryDatas = await _dataRepository.Where(u => dataIds.Contains(u.Id) && u.CategoryId == categoryId).ToListAsync();
            // 判断要操作的角色集合长度是否和传入数组的长度相等
            if (dataIds.Length != dictionaryDatas.Count) throw Oops.Oh(SystemErrorCodes.u1007);

            // 软/假删除
            var nowTime = DateTimeOffset.Now;
            dictionaryDatas.ForEach(u =>
            {
                u.UpdatedTime = nowTime;
                u.IsDeleted = true;
            });
        }
    }
}