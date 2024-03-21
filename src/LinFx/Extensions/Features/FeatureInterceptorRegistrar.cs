using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using System.Reflection;

namespace LinFx.Extensions.Features;

public static class FeatureInterceptorRegistrar
{
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<FeatureInterceptor>();
        }
    }

    private static bool ShouldIntercept(Type type) => !DynamicProxyIgnoreTypes.Contains(type) && (type.IsDefined(typeof(RequiresFeatureAttribute), true) || AnyMethodHasRequiresFeatureAttribute(type));

    private static bool AnyMethodHasRequiresFeatureAttribute(Type implementationType) => implementationType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any(HasRequiresFeatureAttribute);

    private static bool HasRequiresFeatureAttribute(MemberInfo methodInfo) => methodInfo.IsDefined(typeof(RequiresFeatureAttribute), true);
}
