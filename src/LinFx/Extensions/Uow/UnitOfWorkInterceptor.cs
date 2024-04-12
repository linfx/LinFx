using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 工作单元拦截器
/// </summary>
[Service]
public class UnitOfWorkInterceptor(IServiceScopeFactory serviceScopeFactory) : Interceptor
{
    private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory;

    public override async Task InterceptAsync(IMethodInvocation invocation)
    {
        if (!UnitOfWorkHelper.IsUnitOfWorkMethod(invocation.Method, out var unitOfWorkAttribute))
        {
            await invocation.ProceedAsync();
            return;
        }

        using (var scope = serviceScopeFactory.CreateScope())
        {
            var options = CreateOptions(scope.ServiceProvider, invocation, unitOfWorkAttribute);
            var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            //Trying to begin a reserved UOW by UnitOfWorkMiddleware
            if (unitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, options))
            {
                await invocation.ProceedAsync();
                return;
            }

            using (var uow = unitOfWorkManager.Begin(options))
            {
                await invocation.ProceedAsync();
                await uow.CompleteAsync();
            }
        }
    }

    private UnitOfWorkOptions CreateOptions(IServiceProvider serviceProvider, IMethodInvocation invocation, [AllowNull] UnitOfWorkAttribute unitOfWorkAttribute)
    {
        var options = new UnitOfWorkOptions();

        unitOfWorkAttribute?.SetOptions(options);

        if (unitOfWorkAttribute?.IsTransactional == null)
        {
            var defaultOptions = serviceProvider.GetRequiredService<IOptions<UnitOfWorkDefaultOptions>>().Value;
            options.IsTransactional = defaultOptions.CalculateIsTransactional(
                autoValue: serviceProvider.GetRequiredService<IUnitOfWorkTransactionBehaviourProvider>().IsTransactional
                           ?? !invocation.Method.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase)
            );
        }

        return options;
    }
}
