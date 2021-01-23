using Furion.FriendlyException;
using System.ComponentModel;

namespace Pear.Core
{
    /// <summary>
    /// 业务日志错误码
    /// </summary>
    [ErrorCodeType]
    public enum SystemErrorCodes
    {
        /// <summary>
        /// 用户名或密码不正确
        /// </summary>
        [Description("用户名或密码不正确"), ErrorCodeItemMetadata("用户名或密码不正确")]
        u1000,

        /// <summary>
        /// 非法操作！禁止删除自己
        /// </summary>
        [Description("非法操作！禁止删除自己"), ErrorCodeItemMetadata("非法操作！禁止删除自己")]
        u1001,

        /// <summary>
        /// 记录不存在
        /// </summary>
        [Description("记录不存在"), ErrorCodeItemMetadata("记录不存在")]
        u1002,

        /// <summary>
        /// 账号已存在
        /// </summary>
        [Description("账号已存在"), ErrorCodeItemMetadata("账号已存在")]
        u1003,

        /// <summary>
        /// 旧密码不匹配
        /// </summary>
        [Description("旧密码不匹配"), ErrorCodeItemMetadata("旧密码不匹配")]
        u1004,

        /// <summary>
        /// 测试数据禁止更改 admin 密码
        /// </summary>
        [Description("测试数据禁止更改 admin 密码"), ErrorCodeItemMetadata("测试数据禁止更改 admin 密码")]
        u1005,

        /// <summary>
        /// 数据已存在
        /// </summary>
        [Description("数据已存在"), ErrorCodeItemMetadata("数据已存在")]
        u1006,

        /// <summary>
        /// 数据不存在或含有关联引用，禁止删除
        /// </summary>
        [Description("数据不存在或含有关联引用，禁止删除"), ErrorCodeItemMetadata("数据不存在或含有关联引用，禁止删除")]
        u1007,

        /// <summary>
        /// 禁止为管理员分配角色
        /// </summary>
        [Description("禁止为管理员分配角色"), ErrorCodeItemMetadata("禁止为管理员分配角色")]
        u1008,

        /// <summary>
        /// 记录含有不存在数据
        /// </summary>
        [Description("记录含有不存在数据"), ErrorCodeItemMetadata("记录含有不存在数据")]
        u1009,

        /// <summary>
        /// 禁止为超级管理员角色分配权限
        /// </summary>
        [Description("禁止为超级管理员角色分配权限"), ErrorCodeItemMetadata("禁止为超级管理员角色分配权限")]
        u1010,

        /// <summary>
        /// 非法数据
        /// </summary>
        [Description("非法数据"), ErrorCodeItemMetadata("非法数据")]
        u1011,
    }
}