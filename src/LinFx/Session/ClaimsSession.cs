using LinFx.Security;
using System.Linq;

namespace LinFx.Session
{
    public class ClaimsSession : ISession
    {
        protected IPrincipalAccessor PrincipalAccessor { get; }

        public ClaimsSession(IPrincipalAccessor principalAccessor)
        {
            PrincipalAccessor = principalAccessor;
        }

        public string TenantId
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.TenantId);
                return claim?.Value;
            }
        }

        public string UserId
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Id);
                return claim?.Value;
            }
        }
    }
}
