using LinFx.Extensions.DependencyInjection;
using LinFx.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// The default implementation of an  <see cref="IAuthorizationService"/> .
/// </summary>
[Service(ReplaceServices = true)]
public class AuthorizationService(
    IAuthorizationPolicyProvider policyProvider,
    IAuthorizationHandlerProvider handlers,
    ILogger<AuthorizationService> logger,
    IAuthorizationHandlerContextFactory contextFactory,
    IAuthorizationEvaluator evaluator,
    IOptions<AuthorizationOptions> options,
    ICurrentPrincipalAccessor currentPrincipalAccessor,
    IServiceProvider serviceProvider) : DefaultAuthorizationService(policyProvider, handlers, logger, contextFactory, evaluator, options), IAuthorizationService
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor = currentPrincipalAccessor;

    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;
}
