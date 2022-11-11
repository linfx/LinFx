namespace LinFx.Domain.Entities;

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

    void ClearLocalEvents();

    void ClearDistributedEvents();
}
