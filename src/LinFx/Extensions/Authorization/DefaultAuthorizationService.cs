using LinFx.Extensions.DependencyInjection;
using LinFx.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace LinFx.Extensions.Authorization
{
    /// <summary>
    /// The default implementation of an  <see cref="IAuthorizationService"/> .
    /// </summary>
    [Service(ReplaceServices = true)]
    public class DefaultAuthorizationService : Microsoft.AspNetCore.Authorization.DefaultAuthorizationService, IAuthorizationService
    {
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public IServiceProvider ServiceProvider { get; }

        public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;

        public DefaultAuthorizationService(
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers,
            ILogger<DefaultAuthorizationService> logger,
            IAuthorizationHandlerContextFactory contextFactory,
            IAuthorizationEvaluator evaluator,
            IOptions<AuthorizationOptions> options,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IServiceProvider serviceProvider)
            : base(policyProvider, handlers, logger, contextFactory, evaluator, options)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
            ServiceProvider = serviceProvider;
        }
    }
}
