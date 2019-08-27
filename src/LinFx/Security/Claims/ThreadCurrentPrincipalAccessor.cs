using System.Security.Claims;
using System.Threading;

namespace LinFx.Security.Claims
{
    public class ThreadCurrentPrincipalAccessor : IHttpContextPrincipalAccessor
    {
        public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;
    }
}
