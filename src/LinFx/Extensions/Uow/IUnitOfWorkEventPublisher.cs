namespace LinFx.Extensions.Uow;

/// <summary>
/// 领域事件发布器
/// </summary>
public interface IUnitOfWorkEventPublisher
{
    /// <summary>
    /// 发布本地事件
    /// </summary>
    /// <param name="localEvents"></param>
    /// <returns></returns>
    Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents);

    /// <summary>
    /// 发布分步式事件
    /// </summary>
    /// <param name="distributedEvents"></param>
    /// <returns></returns>
    Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents);
}
