using LinFx.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace LinFx.Extensions.Authorization
{
    [Service(ReplaceServices = true)]
    public class DefaultAuthorizationService : Microsoft.AspNetCore.Authorization.DefaultAuthorizationService, IAuthorizationService
    {
        private readonly IHttpContextPrincipalAccessor _currentPrincipalAccessor;

        public IServiceProvider ServiceProvider { get; }

        public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;


        public DefaultAuthorizationService(
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers,
            ILogger<Microsoft.AspNetCore.Authorization.DefaultAuthorizationService> logger,
            IAuthorizationHandlerContextFactory contextFactory,
            IAuthorizationEvaluator evaluator,
            IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions> options,
            IHttpContextPrincipalAccessor currentPrincipalAccessor,
            IServiceProvider serviceProvider)
                : base(policyProvider, handlers, logger, contextFactory, evaluator, options)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
            ServiceProvider = serviceProvider;
        }
    }
}
