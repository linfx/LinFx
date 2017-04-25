using LinFx.Data;
using LinFx.Domain.Repositories;
using LinFx.Domain.Uow;
using System.Threading.Tasks;

namespace LinFx.Zero.Configuration
{
    public class SettingStore
    {
        private readonly IRepository<Setting> _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SettingStore(IRepository<Setting> repository)
        {
            _repository = repository;
        }

        //[UnitOfWork]
        public virtual async Task CreateAsync(Setting item)
        {
            //using (_unitOfWorkManager.Current.SetTenantId(item.TenantId))
            //{
            //using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            //{
                await _repository.InsertAsync(item);
            //await _unitOfWorkManager.Current.SaveChangesAsync();
            //}
            //}
        }
    }
}
