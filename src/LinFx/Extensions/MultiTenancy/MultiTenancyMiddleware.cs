//using LinFx.Extensions.MultiTenancy;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Threading.Tasks;

//namespace LinFx.Extensions.AspNetCore.MultiTenancy
//{
//    public class MultiTenancyMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ITenantResolver _tenantResolver;
//        private readonly ITenantStore _tenantStore;
//        private readonly ICurrentTenantIdAccessor _currentTenantIdAccessor;

//        public MultiTenancyMiddleware(
//            RequestDelegate next,
//            ITenantResolver tenantResolver,
//            ITenantStore tenantStore,
//            ICurrentTenantIdAccessor currentTenantIdAccessor)
//        {
//            _next = next;
//            _tenantResolver = tenantResolver;
//            _tenantStore = tenantStore;
//            _currentTenantIdAccessor = currentTenantIdAccessor;
//        }

//        public async Task Invoke(HttpContext httpContext)
//        {
//            using (SetCurrentTenant(await ResolveCurrentTenantAsync()))
//            {
//                await _next(httpContext);
//            }
//        }

//        private async Task<TenantInfo> ResolveCurrentTenantAsync()
//        {
//            var tenantIdOrName = _tenantResolver.ResolveTenantIdOrName();
//            if (tenantIdOrName == null)
//            {
//                return null;
//            }

//            var tenant = await FindTenantAsync(tenantIdOrName);
//            if (tenant == null)
//            {
//                throw new LinFxException("There is no tenant with given tenant id or name: " + tenantIdOrName);
//            }

//            return tenant;
//        }

//        private Task FindTenantAsync(object tenantIdOrName)
//        {
//            throw new NotImplementedException();
//        }

//        private async Task<TenantInfo> FindTenantAsync(string tenantIdOrName)
//        {
//            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
//            {
//                return await _tenantStore.FindAsync(parsedTenantId);
//            }
//            else
//            {
//                return await _tenantStore.FindAsync(tenantIdOrName);
//            }
//        }

//        private IDisposable SetCurrentTenant([CanBeNull] TenantInfo tenant)
//        {
//            var parentScope = _currentTenantIdAccessor.Current;
//            _currentTenantIdAccessor.Current = new TenantIdWrapper(tenant?.Id);
//            return new DisposeAction(() =>
//            {
//                _currentTenantIdAccessor.Current = parentScope;
//            });
//        }
//    }
//}