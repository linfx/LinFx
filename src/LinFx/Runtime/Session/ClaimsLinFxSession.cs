using LinFx.Security;
using System.Linq;

namespace LinFx.Session
{
    public class ClaimsLinFxSession : ILinFxSession
    {
        protected IPrincipalAccessor PrincipalAccessor { get; }

        public ClaimsLinFxSession(IPrincipalAccessor principalAccessor)
        {
            PrincipalAccessor = principalAccessor;
        }

        public long UserId
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Id);
                long.TryParse(claim.Value, out long val);
                return val;
            }
        }

        public int TenantId
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.TenantId);
                int.TryParse(claim.Value, out int val);
                return val;
            }
        }
    }
}
