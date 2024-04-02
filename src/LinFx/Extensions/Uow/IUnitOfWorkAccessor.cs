using System.Diagnostics.CodeAnalysis;

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
        [AllowNull]
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 设置工作单元
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        void SetUnitOfWork([AllowNull] IUnitOfWork unitOfWork);
    }
}