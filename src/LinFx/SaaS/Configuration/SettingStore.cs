using LinFx.Data;
using System.Threading.Tasks;

namespace LinFx.Zero.Configuration
{
    public class SettingStore
    {
        private readonly IRepository<Setting> _repository;

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
