using LinFx.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计日志配置
/// </summary>
public class AuditingOptions
{
    //TODO: Consider to add an option to disable auditing for application service methods?

    /// <summary>
    /// If this value is true, auditing will not throw an exceptions and it will log it when an error occurred while saving AuditLog.
    /// Default: true.
    /// </summary>
    public bool HideErrors { get; set; }

    /// <summary>
    /// 启用或禁用审计系统的总开关(默认值: true)
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 如果有多个应用程序保存审计日志到单一的数据库,使用此属性设置为你的应用程序名称区分不同的应用程序日志.
    /// </summary>
    public string ApplicationName { get; set; }

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsEnabledForAnonymousUsers { get; set; }

    /// <summary>
    /// Audit log on exceptions.
    /// Default: true.
    /// </summary>
    public bool AlwaysLogOnException { get; set; }

    /// <summary>
    /// 实现的列表. 贡献者是扩展审计日志系统的一种方式. 
    /// </summary>
    public List<AuditLogContributor> Contributors { get; }

    /// <summary>
    /// 审计日志系统忽略的 Type 列表. 
    /// 如果它是实体类型,则不会保存此类型实体的更改. 在序列化操作参数时也使用此列表.
    /// </summary>
    public List<Type> IgnoredTypes { get; }

    /// <summary>
    /// 选择器列表,用于确定是否选择了用于保存实体更改的实体类型. 
    /// </summary>
    public IEntityHistorySelectorList EntityHistorySelectors { get; }

    //TODO: Move this to asp.net core layer or convert it to a more dynamic strategy?
    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsEnabledForGetRequests { get; set; }

    public AuditingOptions()
    {
        IsEnabled = true;
        IsEnabledForAnonymousUsers = true;
        HideErrors = true;
        AlwaysLogOnException = true;

        Contributors = new List<AuditLogContributor>();

        IgnoredTypes = new List<Type>
            {
                typeof(Stream),
                typeof(Expression)
            };

        EntityHistorySelectors = new EntityHistorySelectorList();
    }
}
