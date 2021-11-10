using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace LinFx.Data
{
    public static class ConnectionStringResolverExtensions
    {
        [NotNull]
        public static Task<string> ResolveAsync<T>(this IConnectionStringResolver resolver)
        {
            return resolver.ResolveAsync(typeof(T));
        }

        [NotNull]
        public static Task<string> ResolveAsync(this IConnectionStringResolver resolver, Type type)
        {
            return resolver.ResolveAsync(ConnectionStringNameAttribute.GetConnStringName(type));
        }
    }
}
