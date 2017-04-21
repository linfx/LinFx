namespace LinFx.Domain.Uow
{
    public interface IUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// Unique id of this UOW.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Reference to the outer UOW if exists.
        /// </summary>
        IUnitOfWork Outer { get; set; }

        /// <summary>
        /// Begins the unit of work with given options.
        /// </summary>
        /// <param name="options">Unit of work options</param>
        void Begin(UnitOfWorkOptions options);
    }
}
