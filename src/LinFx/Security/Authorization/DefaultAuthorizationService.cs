//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Security.Claims;

//namespace LinFx.Security.Authorization
//{
//    public class DefaultAuthorizationService : Microsoft.AspNetCore.Authorization.DefaultAuthorizationService
//    {
//        public IServiceProvider ServiceProvider { get; }

//        public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;

//        private readonly IHttpContextPrincipalAccessor _currentPrincipalAccessor;

//        public DefaultAuthorizationService(
//            IAuthorizationPolicyProvider policyProvider,
//            IAuthorizationHandlerProvider handlers,
//            ILogger<Microsoft.AspNetCore.Authorization.DefaultAuthorizationService> logger,
//            IAuthorizationHandlerContextFactory contextFactory,
//            IAuthorizationEvaluator evaluator,
//            IOptions<AuthorizationOptions> options,
//            //ICurrentPrincipalAccessor currentPrincipalAccessor,
//            IServiceProvider serviceProvider)
//            : base(
//                policyProvider,
//                handlers,
//                logger,
//                contextFactory,
//                evaluator,
//                options)
//        {
//            //_currentPrincipalAccessor = currentPrincipalAccessor;
//            ServiceProvider = serviceProvider;
//        }
//    }
//}
