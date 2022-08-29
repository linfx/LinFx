using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Uow;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LinFx.Extensions.AspNetCore.Mvc.Uow;

/// <summary>
/// 工作单元过滤器
/// </summary>
public class UowActionFilter : IAsyncActionFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }

        var methodInfo = context.ActionDescriptor.GetMethodInfo();
        var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);

        //context.HttpContext.Items["_ActionInfo"] = new ActionInfoInHttpContext
        //{
        //    IsObjectResult = context.ActionDescriptor.HasObjectResult()
        //};

        if (unitOfWorkAttr?.IsDisabled == true)
        {
            await next();
            return;
        }

        var options = CreateOptions(context, unitOfWorkAttr);

        var unitOfWorkManager = context.GetRequiredService<IUnitOfWorkManager>();

        //Trying to begin a reserved UOW by UnitOfWorkMiddleware
        if (unitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, options))
        {
            var result = await next();
            if (Succeed(result))
            {
                await SaveChangesAsync(context, unitOfWorkManager);
            }
            else
            {
                await RollbackAsync(context, unitOfWorkManager);
            }

            return;
        }

        using (var uow = unitOfWorkManager.Begin(options))
        {
            var result = await next();
            if (Succeed(result))
            {
                await uow.CompleteAsync(context.HttpContext.RequestAborted);
            }
            else
            {
                await uow.RollbackAsync(context.HttpContext.RequestAborted);
            }
        }
    }

    private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute unitOfWorkAttribute)
    {
        var options = new UnitOfWorkOptions();

        unitOfWorkAttribute?.SetOptions(options);

        if (unitOfWorkAttribute?.IsTransactional == null)
        {
            var unitOfWorkDefaultOptions = context.GetRequiredService<IOptions<UnitOfWorkDefaultOptions>>().Value;
            options.IsTransactional = unitOfWorkDefaultOptions.CalculateIsTransactional(
                autoValue: !string.Equals(context.HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase)
            );
        }

        return options;
    }

    private async Task RollbackAsync(ActionExecutingContext context, IUnitOfWorkManager unitOfWorkManager)
    {
        var currentUow = unitOfWorkManager.Current;
        if (currentUow != null)
        {
            await currentUow.RollbackAsync(context.HttpContext.RequestAborted);
        }
    }

    private async Task SaveChangesAsync(ActionExecutingContext context, IUnitOfWorkManager unitOfWorkManager)
    {
        var currentUow = unitOfWorkManager.Current;
        if (currentUow != null)
        {
            await currentUow.SaveChangesAsync(context.HttpContext.RequestAborted);
        }
    }

    private static bool Succeed(ActionExecutedContext result)
    {
        return result.Exception == null || result.ExceptionHandled;
    }
}
