using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Uow;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.AspNetCore.Uow;

public class UnitOfWorkMiddleware(IUnitOfWorkManager unitOfWorkManager, IOptions<AspNetCoreUnitOfWorkOptions> options) : IMiddleware, ITransientDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager = unitOfWorkManager;
    private readonly AspNetCoreUnitOfWorkOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (IsIgnoredUrl(context))
        {
            await next(context);
            return;
        }

        using var uow = _unitOfWorkManager.Reserve(UnitOfWork.UnitOfWorkReservationName);
        await next(context);
        await uow.CompleteAsync(context.RequestAborted);
    }

    private bool IsIgnoredUrl(HttpContext context) => context.Request.Path.Value != null && _options.IgnoredUrls.Any(x => context.Request.Path.Value.StartsWith(x));
}
