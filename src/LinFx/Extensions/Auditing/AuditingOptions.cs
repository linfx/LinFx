using System.Linq.Expressions;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计日志选项
/// </summary>
public class AuditingOptions
{
    //TODO: Consider to add an option to disable auditing for application service methods?

    /// <summary>
    /// 在保存审计日志对象时如果发生任何错误,审计日志系统会将错误隐藏并写入常规日志. 
    /// 如果保存审计日志对系统非常重要那么将其设置为 false 以便在隐藏错误时抛出异常.
    /// Default: true.
    /// </summary>
    public bool HideErrors { get; set; } = true;

    /// <summary>
    /// 启用或禁用审计系统的总开关(默认值: true)
    /// </summary>
    public bool IsEnabled { get; set; } = false;

    /// <summary>
    /// 如果有多个应用程序保存审计日志到单一的数据库,使用此属性设置为你的应用程序名称区分不同的应用程序日志.
    /// </summary>
    public string ApplicationName { get; set; }

    /// <summary>
    /// 如果只想为经过身份验证的用户记录审计日志,请设置为 false.如果为匿名用户保存审计日志,你将看到这些用户的 UserId 值为 null.
    /// Default: true.
    /// </summary>
    public bool IsEnabledForAnonymousUsers { get; set; } = true;

    /// <summary>
    /// 如果设置为 true,将始终在异常/错误情况下保存审计日志,不检查其他选项(IsEnabled 除外,它完全禁用了审计日志).
    /// Default: true.
    /// </summary>
    public bool AlwaysLogOnException { get; set; } = true;

    /// <summary>
    /// 实现的列表. 贡献者是扩展审计日志系统的一种方式. 
    /// </summary>
    public List<AuditLogContributor> Contributors { get; } = new List<AuditLogContributor>();

    /// <summary>
    /// 审计日志系统忽略的 Type 列表. 
    /// 如果它是实体类型,则不会保存此类型实体的更改. 在序列化操作参数时也使用此列表.
    /// </summary>
    public List<Type> IgnoredTypes { get; } = [typeof(Stream), typeof(Expression)];

    /// <summary>
    /// 选择器列表,用于确定是否选择了用于保存实体更改的实体类型. 
    /// </summary>
    public IEntityHistorySelectorList EntityHistorySelectors { get; } = new EntityHistorySelectorList();

    //TODO: Move this to asp.net core layer or convert it to a more dynamic strategy?
    /// <summary>
    /// HTTP GET请求通常不应该在数据库进行任何更改,审计日志系统不会为GET请求保存审计日志对象. 
    /// 将此值设置为 true 可为GET请求启用审计日志系统.
    /// Default: false.
    /// </summary>
    public bool IsEnabledForGetRequests { get; set; } = false;
}
