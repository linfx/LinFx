using System;
using System.Collections.Generic;
using System.Text;

namespace LinFx.Domain.Uow
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
        ///// <summary>
        ///// Scope option.
        ///// </summary>
        //public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        public bool? IsTransactional { get; private set; }

        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; private set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        //public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// Used to prevent starting a unit of work for the method.
        /// If there is already a started unit of work, this property is ignored.
        /// Default: false.
        /// </summary>
        public bool IsDisabled { get; set; }

        internal UnitOfWorkOptions CreateOptions()
        {
            return new UnitOfWorkOptions
            {
                //IsTransactional = IsTransactional,
                //IsolationLevel = IsolationLevel,
                //Timeout = Timeout,
                //Scope = Scope
            };
        }
    }
}
