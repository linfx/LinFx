using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 工作单元拦截器
/// </summary>
public class UnitOfWorkInterceptor : Interceptor, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UnitOfWorkInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task InterceptAsync(IMethodInvocation invocation)
    {
        if (!UnitOfWorkHelper.IsUnitOfWorkMethod(invocation.Method, out var unitOfWorkAttribute))
        {
            await invocation.ProceedAsync();
            return;
        }

        using (var scope = _serviceScopeFactory.CreateScope())
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

    private UnitOfWorkOptions CreateOptions(IServiceProvider serviceProvider, IMethodInvocation invocation, [CanBeNull] UnitOfWorkAttribute unitOfWorkAttribute)
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
