namespace LinFx.Extensions.Uow;

public interface ISupportsRollback
{
    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackAsync(CancellationToken cancellationToken);
}
