using LinFx.Security.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.MultiTenancy
{
    public class CurrentUserTenantResolveContributor : ITenantResolveContributor
    {
        public const string ContributorName = "CurrentUser";

        public string Name => ContributorName;

        public void Resolve(ITenantResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            if (currentUser.IsAuthenticated != true)
                return;

            context.Handled = true;
            context.TenantIdOrName = currentUser.TenantId?.ToString();
        }
    }
}