namespace LinFx.Extensions.AspNetCore.Uow;

public class AspNetCoreUnitOfWorkOptions
{
    /// <summary>
    /// This is used to disable the <see cref="UnitOfWorkMiddleware"/>, app.UseUnitOfWork(), for the specified URLs.
    /// <see cref="UnitOfWorkMiddleware"/> will be disabled for URLs starting with an ignored URL.  
    /// </summary>
    public List<string> IgnoredUrls { get; } = [];
}
