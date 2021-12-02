namespace LinFx.Extensions.Uow
{
    /// <summary>
    /// 当前活动工作单元
    /// </summary>
    public interface IAmbientUnitOfWork : IUnitOfWorkAccessor
    {
        /// <summary>
        /// 获取当前工作单元
        /// </summary>
        /// <returns></returns>
        IUnitOfWork GetCurrentByChecking();
    }
}