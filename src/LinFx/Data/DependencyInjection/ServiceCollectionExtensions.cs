using LinFx.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static LinFxBuilder AddDataFilter(this LinFxBuilder builder)
    {
        builder.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        return builder;
    }
}
