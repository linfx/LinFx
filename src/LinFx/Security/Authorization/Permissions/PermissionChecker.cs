using LinFx.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限检查器
    /// </summary>
    public class PermissionChecker : IPermissionChecker
    {
        private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

        /// <summary>
        /// 权限管理器
        /// </summary>
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        /// <summary>
        /// 权限值提供者
        /// </summary>
        protected IReadOnlyList<IPermissionValueProvider> ValueProviders => _lazyProviders.Value;

        protected IHttpContextPrincipalAccessor PrincipalAccessor { get; }

        protected PermissionOptions Options { get; }

        public PermissionChecker(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider,
            IHttpContextPrincipalAccessor principalAccessor,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            PrincipalAccessor = principalAccessor;
            PermissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;

            _lazyProviders = new Lazy<List<IPermissionValueProvider>>(() => Options
                    .ValueProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as IPermissionValueProvider)
                    .ToList(), true);
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Task<PermissionGrantInfo> CheckAsync(string name)
        {
            return CheckAsync(PrincipalAccessor.Principal, name);
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<PermissionGrantInfo> CheckAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            Check.NotNull(name, nameof(name));

            var context = new PermissionValueCheckContext(PermissionDefinitionManager.Get(name), claimsPrincipal);

            foreach (var provider in ValueProviders)
            {
                if (context.Permission.Providers.Any() && !context.Permission.Providers.Contains(provider.Name))
                    continue;

                var result = await provider.CheckAsync(context);
                if (result.IsGranted)
                    return new PermissionGrantInfo(context.Permission.Name, true, provider.Name, result.ProviderKey);
            }

            return new PermissionGrantInfo(context.Permission.Name, false);
        }
    }
}