using LinFx.Extensions.ObjectExtending;
using System.Text;

namespace LinFx.Extensions.Auditing;

/// <summary>
///  审计信息
/// </summary>
[Serializable]
public class AuditLogInfo : IHasExtraProperties
{
    /// <summary>
    /// 当你保存不同的应用审计日志到同一个数据库,这个属性用来区分应用程序.
    /// </summary>
    public string ApplicationName { get; set; }

    /// <summary>
    /// 当前用户的Id,用户未登录为 null.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 当前用户的用户名,如果用户已经登录(这里的值不依赖于标识模块/系统进行查找).
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 当前租户的Id.
    /// </summary>
    public string TenantId { get; set; }

    /// <summary>
    /// 当前租户的名称.
    /// </summary>
    public string TenantName { get; set; }

    public string ImpersonatorUserId { get; set; }

    public string ImpersonatorTenantId { get; set; }

    /// <summary>
    /// 审计日志对象创建的时间.
    /// </summary>
    public DateTimeOffset ExecutionTime { get; set; }

    /// <summary>
    /// 请求的总执行时间,以毫秒为单位. 可以用来观察应用程序的性能.
    /// </summary>
    public int ExecutionDuration { get; set; }

    /// <summary>
    /// 当前客户端的Id,如果客户端已经通过认证.客户端通常是使用HTTP API的第三方应用程序.
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 当前相关Id. 相关Id用于在单个逻辑操作中关联由不同应用程序(或微服务)写入的审计日志.
    /// </summary>
    public string CorrelationId { get; set; }

    /// <summary>
    /// 客户端/用户设备的IP地址.
    /// </summary>
    public string ClientIpAddress { get; set; }

    /// <summary>
    /// 当前客户端的Id,如果客户端已经通过认证.客户端通常是使用HTTP API的第三方应用程序.
    /// </summary>
    public string ClientName { get; set; }

    /// <summary>
    /// 当前用户的浏览器名称/版本信息.
    /// </summary>
    public string BrowserInfo { get; set; }

    /// <summary>
    /// 当前HTTP请求的方法
    /// </summary>
    public string HttpMethod { get; set; }

    /// <summary>
    /// HTTP响应状态码.
    /// </summary>
    public int? HttpStatusCode { get; set; }

    /// <summary>
    /// 请求Url
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 执行的动作 (控制器操作和应用服务方法调用及其参数).
    /// </summary>
    public List<AuditLogActionInfo> Actions { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public List<Exception> Exceptions { get; }

    public ExtraPropertyDictionary ExtraProperties { get; }

    /// <summary>
    /// 实体的变化 (在Web请求中).
    /// </summary>
    public List<EntityChangeInfo> EntityChanges { get; }

    public List<string> Comments { get; set; }

    public AuditLogInfo()
    {
        Actions = [];
        Exceptions = [];
        ExtraProperties = [];
        EntityChanges = [];
        Comments = [];
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"AUDIT LOG: [{HttpStatusCode?.ToString() ?? "---"}: {HttpMethod ?? "-------",-7}] {Url}");
        sb.AppendLine($"- UserName - UserId                 : {UserName} - {UserId}");
        sb.AppendLine($"- ClientIpAddress        : {ClientIpAddress}");
        sb.AppendLine($"- ExecutionDuration      : {ExecutionDuration}");

        if (Actions.Any())
        {
            sb.AppendLine("- Actions:");
            foreach (var action in Actions)
            {
                sb.AppendLine($"  - {action.ServiceName}.{action.MethodName} ({action.ExecutionDuration} ms.)");
                sb.AppendLine($"    {action.Parameters}");
            }
        }

        if (Exceptions.Any())
        {
            sb.AppendLine("- Exceptions:");
            foreach (var exception in Exceptions)
            {
                sb.AppendLine($"  - {exception.Message}");
                sb.AppendLine($"    {exception}");
            }
        }

        if (EntityChanges.Any())
        {
            sb.AppendLine("- Entity Changes:");
            foreach (var entityChange in EntityChanges)
            {
                sb.AppendLine($"  - [{entityChange.ChangeType}] {entityChange.EntityTypeFullName}, Id = {entityChange.EntityId}");
                foreach (var propertyChange in entityChange.PropertyChanges)
                {
                    sb.AppendLine($"    {propertyChange.PropertyName}: {propertyChange.OriginalValue} -> {propertyChange.NewValue}");
                }
            }
        }

        return sb.ToString();
    }
}
