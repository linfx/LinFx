using LinFx.Authorization.Accounts;
using LinFx.Data;

namespace LinFx.SaaS.Authorization
{
    public class AccountManager : LinFx.Authorization.Accounts.AccountManager
    {
        public AccountManager(IRepository<Account> repository) : base(repository)
        {
        }

        ////[UnitOfWork]
        //public virtual async Task<LoginResult<Tenant, User>> LoginAsync(string userName, string password)
        //{
        //    var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
        //    //await SaveLoginAttempt(result, tenancyName, userNameOrEmailAddress);
        //    return result;
        //}

        //protected virtual async Task<LoginResult<Tenant, User>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        //{
        //    if (string.IsNullOrEmpty(userNameOrEmailAddress))
        //    {
        //        throw new ArgumentNullException(nameof(userNameOrEmailAddress));
        //    }

        //    if (string.IsNullOrEmpty(plainPassword))
        //    {
        //        throw new ArgumentNullException(nameof(plainPassword));
        //    }

        //    //Get and check tenant
        //    TTenant tenant = null;
        //    using (UnitOfWorkManager.Current.SetTenantId(null))
        //    {
        //        if (!MultiTenancyConfig.IsEnabled)
        //        {
        //            tenant = await GetDefaultTenantAsync();
        //        }
        //        else if (!string.IsNullOrWhiteSpace(tenancyName))
        //        {
        //            tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
        //            if (tenant == null)
        //            {
        //                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
        //            }

        //            if (!tenant.IsActive)
        //            {
        //                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
        //            }
        //        }
        //    }

        //    var tenantId = tenant == null ? (int?)null : tenant.Id;
        //    using (UnitOfWorkManager.Current.SetTenantId(tenantId))
        //    {
        //        //TryLoginFromExternalAuthenticationSources method may create the user, that's why we are calling it before AbpStore.FindByNameOrEmailAsync
        //        var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(userNameOrEmailAddress, plainPassword, tenant);

        //        var user = await UserManager.AbpStore.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
        //        if (user == null)
        //        {
        //            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
        //        }

        //        if (!loggedInFromExternalSource)
        //        {
        //            UserManager.InitializeLockoutSettings(tenantId);

        //            if (await UserManager.IsLockedOutAsync(user.Id))
        //            {
        //                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, user);
        //            }

        //            var verificationResult = UserManager.PasswordHasher.VerifyHashedPassword(user.Password, plainPassword);
        //            if (verificationResult != PasswordVerificationResult.Success)
        //            {
        //                if (shouldLockout)
        //                {
        //                    if (await TryLockOutAsync(tenantId, user.Id))
        //                    {
        //                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, user);
        //                    }
        //                }

        //                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, user);
        //            }

        //            await UserManager.ResetAccessFailedCountAsync(user.Id);
        //        }

        //        return await CreateLoginResultAsync(user, tenant);
        //    }
        //}

    }
}
