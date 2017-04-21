namespace LinFx.Domain.Uow
{
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        public bool? IsTransactional { get; set; }
    }
}
