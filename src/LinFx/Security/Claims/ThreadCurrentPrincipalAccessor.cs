using LinFx.Extensions.DependencyInjection;
using System.Security.Claims;

namespace LinFx.Security.Claims;

public class ThreadCurrentPrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
{
    public virtual ClaimsPrincipal Principal => (Thread.CurrentPrincipal as ClaimsPrincipal)!;
}
