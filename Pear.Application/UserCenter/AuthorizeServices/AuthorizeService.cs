using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pear.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pear.Application.UserCenter
{
    /// <summary>
    /// 授权服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroupConsts.USER_CENTER, Name = "auth", Order = 100)]
    public class AuthorizeService : IAuthorizeService, IDynamicApiController, ITransient
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// 用户管理类
        /// </summary>
        private readonly IUserManager _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="userManager"></param>
        public AuthorizeService(IRepository<User> userRepository
            , IUserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="input"></param>
        /// <remarks>
        /// 用户名/密码：admin/admin
        /// </remarks>
        /// <returns></returns>
        [AllowAnonymous, ApiDescriptionSettings(Order = 100)]
        public async Task<LoginOutput> LoginAsync([FromServices] IHttpContextAccessor httpContextAccessor, [Required] LoginInput input)
        {
            // 获取加密后的密码
            var encryptPassword = MD5Encryption.Encrypt(input.Password.Trim());

            // 判断用户名或密码是否正确
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Account.Equals(input.Account) && u.Password.Equals(encryptPassword));
            _ = user ?? throw Oops.Oh(SystemErrorCodes.u1000);

            // 更新登录时间
            user.SigninedTime = DateTimeOffset.Now;

            // 映射结果
            var output = user.Adapt<LoginOutput>();

            // 生成 token
            var accessToken = output.AccessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { "UserId",user.Id },
                { "Account",user.Account }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            httpContextAccessor.SigninToSwagger(accessToken);

            // 设置刷新 token
            httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return output;
        }
    }
}