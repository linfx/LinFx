namespace LinFx.Extensions.Data;

public static class ConnectionStringResolverExtensions
{
    public static Task<string> ResolveAsync<T>(this IConnectionStringResolver resolver) => resolver.ResolveAsync(typeof(T));

    public static Task<string> ResolveAsync(this IConnectionStringResolver resolver, Type type) => resolver.ResolveAsync(ConnectionStringNameAttribute.GetConnStringName(type));
}
