namespace LinFx.Extensions.Auditing;

public interface IAuditLogSaveHandle : IDisposable
{
    /// <summary>
    /// 保存审计
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
