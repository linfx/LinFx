using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Features;
using System.Reflection;

namespace LinFx.Extensions.Features;

public class MethodInvocationFeatureCheckerService(IFeatureChecker featureChecker) : IMethodInvocationFeatureCheckerService, ITransientDependency
{
    private readonly IFeatureChecker _featureChecker = featureChecker;

    public async Task CheckAsync(MethodInvocationFeatureCheckerContext context)
    {
        if (IsFeatureCheckDisabled(context))
            return;

        foreach (var requiresFeatureAttribute in GetRequiredFeatureAttributes(context.Method))
        {
            await _featureChecker.CheckEnabledAsync(requiresFeatureAttribute.RequiresAll, requiresFeatureAttribute.Features);
        }
    }

    protected virtual bool IsFeatureCheckDisabled(MethodInvocationFeatureCheckerContext context) => context.Method.GetCustomAttributes(true).OfType<DisableFeatureCheckAttribute>().Any();

    protected virtual IEnumerable<RequiresFeatureAttribute> GetRequiredFeatureAttributes(MethodInfo methodInfo)
    {
        var attributes = methodInfo.GetCustomAttributes(true).OfType<RequiresFeatureAttribute>();

        if (methodInfo.IsPublic)
            attributes = attributes.Union(methodInfo.DeclaringType!.GetCustomAttributes(true).OfType<RequiresFeatureAttribute>());

        return attributes;
    }
}
