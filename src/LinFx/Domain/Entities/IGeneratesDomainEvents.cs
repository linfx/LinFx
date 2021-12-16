using System.Collections.Generic;

namespace LinFx.Domain.Entities;

//TODO: Re-consider this interface

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
