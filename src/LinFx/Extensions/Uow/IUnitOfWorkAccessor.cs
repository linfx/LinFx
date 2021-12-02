using JetBrains.Annotations;

namespace LinFx.Extensions.Uow
{
    /// <summary>
    /// 工作单元访问器
    /// </summary>
    public interface IUnitOfWorkAccessor
    {
        /// <summary>
        /// 当前工作单元
        /// </summary>
        [CanBeNull]
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 设置工作单元
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        void SetUnitOfWork([CanBeNull] IUnitOfWork unitOfWork);
    }
}