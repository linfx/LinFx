using LinFx.Threading;

namespace Microsoft.Extensions.DependencyInjection;

public static class ThreadingServiceCollectionExtensions
{
    public static LinFxBuilder AddThreading(this LinFxBuilder builder)
    {
        builder.Services.AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);
        builder.Services.AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>));
        return builder;
    }
}
