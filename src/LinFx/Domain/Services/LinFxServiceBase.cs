using LinFx.Configuration;
using LinFx.Domain.Uow;
using LinFx.ObjectMapping;
using LinFx.Session;
using Microsoft.Extensions.Logging;

namespace LinFx.Domain.Services
{
    public abstract class LinFxServiceBase
    {
        private IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager"/>.
        /// </summary>
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                    throw new LinFxException("Must set UnitOfWorkManager before use it.");

                return _unitOfWorkManager;
            }
            set { _unitOfWorkManager = value; }
        }
		/// <summary>
		/// Reference to the logger to write logs.
		/// </summary>
		public ILogger Logger { get; set; }
        /// <summary>
        /// Reference to the setting manager.
        /// </summary>
        public ISettingManager SettingManager { get; set; }
        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }
        /// <summary>
        /// Reference to current session.
        /// </summary>
        public ILinFxSession Session { protected get; set; } = new ClaimsLinFxSession(HttpContext.PrincipalAccessor);
        /// <summary>
        /// Gets current unit of work.
        /// </summary>
        protected IActiveUnitOfWork CurrentUnitOfWork { get { return UnitOfWorkManager.Current; } }
    }
}
