using LinFx.Domain.Uow;
using LinFx.Logging;
using System;

namespace LinFx
{
    public abstract class ServiceBase
    {
        private IUnitOfWorkManager _unitOfWorkManager;

        public ServiceBase()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// Reference to the logger to write logs.
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager"/>.
        /// </summary>
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                {
                    throw new Exception("Must set UnitOfWorkManager before use it.");
                }
                return _unitOfWorkManager;
            }
            set { _unitOfWorkManager = value; }
        }

        /// <summary>
        /// Gets current unit of work.
        /// </summary>
        protected IActiveUnitOfWork CurrentUnitOfWork { get { return UnitOfWorkManager.Current; } }
    }
}
