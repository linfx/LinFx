namespace LinFx.Domain.Entities;

/// <summary>
/// 领域事件
/// </summary>
public interface IGeneratesDomainEvents
{
    /// <summary>
    /// 获得所有本地事件
    /// </summary>
    /// <returns></returns>
    IEnumerable<DomainEventRecord> GetLocalEvents();

    /// <summary>
    /// 获得所有本地事件
    /// </summary>
    /// <returns></returns>
    IEnumerable<DomainEventRecord> GetDistributedEvents();

    /// <summary>
    /// 清除本地领域事件
    /// </summary>
    void ClearLocalEvents();

    /// <summary>
    /// 清除分步式领域事件
    /// </summary>
    void ClearDistributedEvents();
}
