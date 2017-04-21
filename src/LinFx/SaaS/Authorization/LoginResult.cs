using LinFx.SaaS.Authorization.Users;
using LinFx.SaaS.MultiTenancy;

namespace LinFx.SaaS.Authorization
{
    public class LoginResult<TTenant, TUser>
        where TTenant : Tenant
        where TUser : User
    {
        public TTenant Tenant { get; private set; }

        public TUser User { get; private set; }

        //public ClaimsIdentity Identity { get; private set; }
    }
}
