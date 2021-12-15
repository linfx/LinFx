using System.Data;

namespace LinFx.Extensions.Uow
{
    /// <summary>
    /// 工作单元配置
    /// </summary>
    public interface IUnitOfWorkOptions
    {
        /// <summary>
        /// 是否事务
        /// </summary>
        bool IsTransactional { get; }

        IsolationLevel? IsolationLevel { get; }

        /// <summary>
        /// Milliseconds
        /// </summary>
        int? Timeout { get; }
    }
}
