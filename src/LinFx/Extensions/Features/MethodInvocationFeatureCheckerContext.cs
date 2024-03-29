using System.Reflection;

namespace LinFx.Extensions.Features;

public class MethodInvocationFeatureCheckerContext(MethodInfo method)
{
    public MethodInfo Method { get; } = method;
}
