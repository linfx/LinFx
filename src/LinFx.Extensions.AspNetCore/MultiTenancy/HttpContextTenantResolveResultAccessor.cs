using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using Microsoft.AspNetCore.Http;

namespace LinFx.Extensions.AspNetCore.MultiTenancy
{
    [Service(ReplaceServices = true)]
    public class HttpContextTenantResolveResultAccessor : ITenantResolveResultAccessor, ITransientDependency
    {
        public const string HttpContextItemName = "TenantResolveResult";

        public TenantResolveResult Result
        {
            get => _httpContextAccessor.HttpContext?.Items[HttpContextItemName] as TenantResolveResult;
            set
            {
                if (_httpContextAccessor.HttpContext == null)
                    return;

                _httpContextAccessor.HttpContext.Items[HttpContextItemName] = value;
            }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTenantResolveResultAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
